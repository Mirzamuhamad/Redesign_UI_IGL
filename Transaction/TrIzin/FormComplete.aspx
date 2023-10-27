<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormComplete.Aspx.vb" Inherits="FormComplete" %>

<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>     

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Form</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
        .style1
        {
            width: 409px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    </div>
    <div class="Content"> <%--nambahin ini doank--%>
        <asp:Panel runat="server" ID="pnlDt">
        <table style="background-color: #1085C7; color: #FFFFFF;">
            <tr> 
                <td class="style1">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                Complete Izin Istirahat / Pulang Awal</div>   
                </td>         
            </tr>
        </table>
       <asp:Panel runat="server" ID="pnlEditDt">
            <table>
                <tr>
                    <td>Employee</td>
                    <td>:</td>
                    <td>                    
                        <asp:Label ID="lblEmpNo" runat="server" Text="EmpNo"></asp:Label>
                     </td>
                </tr>
                <tr>
                    <td>Start Hour</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbStartHour" runat="server" CssClass="TextBox" MaxLength="5" Width="45px" />
                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="tbStartHour"
                                Mask="99:99"
                                MaskType="Time"
                                CultureName="en-us"
                                MessageValidatorTip="true"
                                runat="server">
                    </cc1:MaskedEditExtender>
                    </td>            
                </tr>                      
                <tr>
                    <td>End Hour</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbEndHour" runat="server" CssClass="TextBox" MaxLength="5" Width="45px" />
                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="tbEndHour"
                                Mask="99:99"
                                MaskType="Time"
                                CultureName="en-us"
                                MessageValidatorTip="true"
                                runat="server">
                    </cc1:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Catatan Dokter</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" MaxLength="50" 
                            Width="298px" />
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
