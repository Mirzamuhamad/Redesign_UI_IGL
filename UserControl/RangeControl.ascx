<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RangeControl.ascx.vb" Inherits="UserControl_RangeControl" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
<script src="../../Function/Function.js" type="text/javascript"></script>

        <fieldset style="width:260px">
            <legend>Range</legend>
                <table>
                    <tr>
                        <td>Range 1</td>
                        <td>:</td>
                        <td>&nbsp</td>
                        <td><=</td>
                        <td><asp:TextBox ID="tb1" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="30" runat="server"/></td>
                    </tr>
                    <tr>
                        <td>Range 2</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStart2" Width="50px" Enabled ="false" Text="31" CssClass="TextBox" runat="server"></asp:TextBox></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tb2" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="60" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Range 3</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStart3" Width="50px" Enabled ="false" Text="61" CssClass="TextBox" runat="server"></asp:TextBox></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tb3" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="90" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Range 4</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStart4" Width="50px" Enabled ="false" Text="91" CssClass="TextBox" runat="server"></asp:TextBox></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tb4" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="120" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Range 5</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStart5" Width="50px" Enabled ="false" Text="121" CssClass="TextBox" runat="server"></asp:TextBox></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tb5" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="150" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Range 6</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStart6" Width="50px" Enabled ="false" Text="151" CssClass="TextBox" runat="server"></asp:TextBox></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tb6" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="180" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Range 7</td>
                        <td>:</td>
                        <td>&nbsp</td>
                        <td>>=</td>
                        <td><asp:TextBox ID="tb7" Width="50px" CssClass="TextBox" Text="180" Enabled ="false" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
        </fieldset>
        <br />
<asp:Label ID="lStatus" runat="server" ForeColor="Red" /> 