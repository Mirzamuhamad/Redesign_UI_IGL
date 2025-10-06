<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RangeControl2.ascx.vb" Inherits="UserControl_RangeControl2" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">

        <fieldset style="width:260px">
            <legend>Range</legend>
                <table>
                    <tr>
                        <td>Range 1</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tb1" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="30" runat="server"/></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tbStart1" Width="50px" Enabled ="false" Text="59" CssClass="TextBox" runat="server"/></td>
                    </tr>
                    <tr>
                        <td>Range 2</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tb2" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="60" runat="server"/></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tbStart2" Width="50px" Enabled ="false" Text="89" CssClass="TextBox" runat="server"/></td>
                    </tr>
                    <tr>
                        <td>Range 3</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tb3" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="90" runat="server"/></td>
                        <td>Until</td>
                        <td><asp:TextBox ID="tbStart3" Width="50px" Enabled ="false" Text="119" CssClass="TextBox" runat="server"/></td>
                    </tr>
                    <tr>
                        <td>Range 4</td>
                        <td>:</td>
                        <td>&nbsp</td>
                        <td>>=</td>
                        <td><asp:TextBox ID="tb4" AutoPostBack="true" Width="50px" CssClass="TextBox" Text="120" runat="server"/></td>
                    </tr>
                </table>
        </fieldset>
        <br />
<asp:Label ID="lStatus" runat="server" ForeColor="Red" /> 