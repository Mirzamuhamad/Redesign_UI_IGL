<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormComplete.Aspx.vb" Inherits="FormComplete" %>

<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Form</title>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
    <style type="text/css">
        .style1
        {
            width: 409px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content"> <%--nambahin ini doank--%>
        <asp:Panel runat="server" ID="pnlDt">
        <table style="background-color: #1085C7; color: #FFFFFF;" width = "733px">
            <tr> 
                <td class="style1">
                <div style="border-style: solid; border-color: inherit; border-width: 0px; width:692%; height:13px; overflow:auto;">
                    Complete Overtime</div>   
                </td>         
            </tr>
        </table>
       <asp:Panel runat="server" ID="pnlEditDt">
            <table style="width: 733px">
                <tr>
                    <td>Overtime No</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:Label ID="lblOvertime" runat="server" Text="OvertimeNo"></asp:Label>
                     </td>
                    <td rowspan="12">
                        <table frame="border" style="border-style: solid; width: 136px;">
                            <tr align="center">
                                <td bgcolor="#cccccc" colspan="3">
                                    Info Absence</td>
                            </tr>
                            <tr>
                                <td>
                                    Time In</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbTimeIn" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="53px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Time Out</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="TbTimeOut" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="53px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>Overtime Date</td>
                    <td>:</td>
                    <td colspan="4">
                        <BDP:BasicDatePicker ID="tbDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" Enabled = "false">
                            <TextBoxStyle CssClass="TextDate"/>
                        </BDP:BasicDatePicker>
                    </td>            
                </tr>                      
                <tr>
                    <td>Start Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" Enabled ="false">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Start Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbStartTime" runat="server" CssClass="TextBoxR" MaxLength="5" Width="53px" Enabled = "false"/>
                    </td>
                </tr>
                <tr>
                    <td>End Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" Enabled = "false">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">End Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbEndTime" runat="server" CssClass="TextBox" MaxLength="5" Enabled = "false" Width= "53px" />
                    </td>
                </tr>
                <tr>
                    <td>Minute</td>
                    <td>:</td>
                    <td colspan="2">
                    <table>
                        <tr align = "center">
                        <td bgcolor = "#cccccc">Minute Bruto</td>
                        <td bgcolor = "#cccccc">Minute Break</td>
                        <td bgcolor = "#cccccc">Minute Netto</td>
                        </tr>
                        <tr>
                        <td><asp:TextBox ID="tbMinuteBruto" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" /></td>
                        <td><asp:TextBox ID="tbMinuteBreak" runat="server" CssClass="TextBoxR" Enabled = "false" Width="67px" /></td>
                        <td><asp:TextBox ID="tbMinuteNetto" runat="server" CssClass="TextBoxR" Enabled="false" Width="68px" /></td>
                        </tr>
                    </table>
                        </td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>Employee</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbEmpNo" runat="server" CssClass="TextBoxR" MaxLength="15" 
                            Width="116px" Enabled="False" />
                        <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="False" MaxLength="255" Width="325px" />
                    </td>
                </tr>
                <tr>
                    <td>Day Type</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlDayType" runat="server" CssClass="DropDownList" 
                            Height="17px"  Width="91px" Enabled = "false">
                            <asp:ListItem>Work</asp:ListItem>
                            <asp:ListItem>Holiday</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Act Start Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbActStartDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            ValidationGroup="Input" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Act Start Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActStartTime" runat="server" AutoPostBack="True" CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px" />
                    </td>
                </tr>
                <tr>
                    <td>Act End Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbActEndDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            ValidationGroup="Input" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Act End Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActEndTime" runat="server" AutoPostBack="True" CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px" />
                    </td>
                </tr>
                <tr>
                    <td>Minute</td>
                    <td>:</td>
                    <td colspan="2">
                    <table>
                        <tr align ="center">
                            <td bgColor="#cccccc">Minute Bruto</td>
                            <td bgColor="#cccccc">Minute Break</td>
                            <td bgColor="#cccccc">Minute Netto</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbActMinuteBruto" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" /></td>
                            <td><asp:TextBox ID="tbActMinuteBreak" runat="server" AutoPostBack="True" CssClass="TextBox" ValidationGroup="Input" Width="67px" /></td>
                            <td><asp:TextBox ID="tbActMinuteNetto" runat="server" CssClass="TextBoxR" Enabled="false" Width="68px" />
                            </td>
                        </tr>
                    </table>
                    </td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>Hour Netto</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActHournetto" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                    </td>
                    <td align = "right">OT Hour</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbOTHour" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                    </td>
                </tr>
                <tr>
                    <td>Act Meal Allowance</td>
                    <td>:</td>
                    <td colspan="4"><asp:DropDownList ID="ddlActFgMealAllowance" runat="server" CssClass="DropDownList" Height="23px" ValidationGroup="Input" Width="41px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />                     
       </asp:Panel>           
            <asp:Button ID="btnOK2" runat="server" class="bitbtn btnok" Text="OK" Width="59px" />
            <asp:Button ID="btnCancel2" runat="server" class="bitbtn btncancel" Text="Cancel" Width="59px" />
            <asp:Button ID="btnBack2" runat="server" class="bitbtn btnback" Text="Back" 
                Width="59px" Visible="False" />           
       </asp:Panel>          
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
