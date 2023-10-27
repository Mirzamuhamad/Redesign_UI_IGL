<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCompInfo.aspx.vb" Inherits="Transaction_TrCompInfo_TrCompInfo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="BasicFrame.WebControls.BasicDatePicker" namespace="BasicFrame.WebControls" tagprefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company Information</title>
        <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
   </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style3
        {
        }
        .style4
        {
            width: 96px;
        }
        .style5
        {
            width: 2px;
        }
        .style6
        {
        }
        .style7
        {
            width: 99px;
        }
        .style8
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1">Company Information</div>
     <hr style="color:Blue" />
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td class="style7">Company Name</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCompanyName" CssClass="TextBox" MaxLength="100" 
                        runat="server" Width="353px" /></td>                
            </tr>
            <tr>
                <td class="style7">Company Alias</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style6" colspan="3">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Report : &nbsp;<asp:TextBox ID="tbReport" runat="server" CssClass="TextBox" 
                        MaxLength="10" Width="100" />
                    &nbsp;&nbsp;&nbsp; Non Report :
                    <asp:TextBox ID="tbNonReport" runat="server" CssClass="TextBox" MaxLength="10" 
                        Width="100" />
                </td>
            </tr>
            <tr>
                <td class="style3" colspan="3">
                <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
                    <StaticSelectedStyle CssClass="MenuSelect" />
                    <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Office" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Branch" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
                <br />
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Tab1" runat="server">
                            <table width="100%"><tr> <td class="style4">  Address Line (1)</td>
                            <td class="style5">  :</td>
                            <td>  
                                <asp:TextBox ID="tbAddr1" runat="server" CssClass="TextBox" MaxLength="100" 
                                    Width="350" />
                                </td></tr>
                                <tr>
                                    <td class="style4">
                                        Address Line (2)</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbAddr2" runat="server" CssClass="TextBox" MaxLength="100" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        City</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbCity" runat="server" CssClass="TextBox" MaxLength="60" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Country</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbCountry" runat="server" CssClass="TextBox" MaxLength="60" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Postal Code</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbZipCode" runat="server" CssClass="TextBox" MaxLength="10" 
                                            Width="77px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Phone</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbPhone" runat="server" CssClass="TextBox" MaxLength="40" 
                                            Width="168px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Fax</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbFax" runat="server" CssClass="TextBox" MaxLength="40" 
                                            Width="168px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="Tab2" runat="server">
                            <table width="100%"><tr> <td class="style4">  Address Line (1)</td>
                            <td class="style5">  :</td>
                            <td>  
                                <asp:TextBox ID="tbBranchAddr1" runat="server" CssClass="TextBox" MaxLength="100" 
                                    Width="350" />
                                </td></tr>
                                <tr>
                                    <td class="style4">
                                        Address Line (2)</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchAddr2" runat="server" CssClass="TextBox" MaxLength="100" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        City</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchCity" runat="server" CssClass="TextBox" MaxLength="60" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Country</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchCountry" runat="server" CssClass="TextBox" MaxLength="60" 
                                            Width="350" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Postal Code</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchZipCode" runat="server" CssClass="TextBox" MaxLength="10" 
                                            Width="77px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Phone</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchPhone" runat="server" CssClass="TextBox" MaxLength="40" 
                                            Width="168px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        Fax</td>
                                    <td class="style5">
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbBranchFax" runat="server" CssClass="TextBox" MaxLength="60" 
                                            Width="168px" />
                                    </td>
                                </tr>
                            </table>
                         </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
             <tr><td class="style7">Email</td><td> :</td><td >
                 <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" MaxLength="60" 
                     Width="350" />
                 </td>
            </tr>
            <tr>
                <td class="style7">
                    Web Page</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbWeb" runat="server" CssClass="TextBox" MaxLength="60" 
                        Width="350" />
                </td>
            </tr>
            <tr>
                <td class="style7">
                    NPKP</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbNPKP" runat="server" CssClass="TextBox" MaxLength="25" 
                        Width="168px" />
                </td>
            </tr>
            <tr>
                <td class="style7">
                    MOTO</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbMoto" runat="server" CssClass="TextBox" MaxLength="500" 
                        Width="350" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <table class="style8">
                        <tr>
                            <td>
                                NPWP</td>
                            <td>
                                Effective Date</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbNpwp" runat="server" CssClass="TextBox" MaxLength="25" 
                                    Width="277px" />
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbNpwp2" runat="server" CssClass="TextBox" MaxLength="25" 
                                    Width="277px" />
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbEffectiveDate2" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbNpwp3" runat="server" CssClass="TextBox" MaxLength="25" 
                                    Width="277px" />
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbEffectiveDate3" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbNpwp4" runat="server" CssClass="TextBox" MaxLength="25" 
                                    Width="277px" />
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbEffectiveDate4" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                &nbsp;<asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save"/>									
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" Visible="False" />		
                </td>
            </tr>
        </table>
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
