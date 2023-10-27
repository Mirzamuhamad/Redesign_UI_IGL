<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdvanceFind.ascx.vb" Inherits="UserControl_AdvanceFind" %>
<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>

<fieldset style="width:480px; height: 204px;">
    <legend>Advanced Search</legend>
	<table>
	    <tr>
	        <td>
	            <asp:DropDownList ID="ddlOperatorDate1" AutoPostBack="true" runat="server">
	                <asp:ListItem Selected="True">-</asp:ListItem>
	                <asp:ListItem>OR</asp:ListItem>
	                <asp:ListItem>AND</asp:ListItem>	                
	            </asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlDateField1" Enabled ="false" runat="server"></asp:DropDownList>
	        </td>
	        <td>
	            <cc1:GMDatePicker ID="tbStartDate1" Enabled ="false" runat="server" DisplayMode="Label">
                </cc1:GMDatePicker>	            
	            s/d
	            <cc1:GMDatePicker ID="tbEndDate1" Enabled ="false" runat="server" DisplayMode="Label">
                </cc1:GMDatePicker>
	        </td>
	    </tr>
	    <tr>
	        <td>
	            <asp:DropDownList ID="ddlOperatorDate2" AutoPostBack="true" runat="server">
	                <asp:ListItem Selected="True">-</asp:ListItem>
	                <asp:ListItem>OR</asp:ListItem>
	                <asp:ListItem>AND</asp:ListItem>	                
	            </asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlDateField2" Enabled ="false" runat="server"></asp:DropDownList>
	        </td>
	        <td>
	            <cc1:GMDatePicker ID="tbStartDate2" Enabled ="false" runat="server" 
                    DisplayMode="Label">
                </cc1:GMDatePicker>	            
	            s/d
	            <cc1:GMDatePicker ID="tbEndDate2" Enabled ="false" runat="server" DisplayMode="Label">
                </cc1:GMDatePicker>
	        </td>
	    </tr>	    
	    <tr>
	        <td>
	            <asp:DropDownList ID="ddlOperator1" AutoPostBack="true" runat="server">
	                <asp:ListItem Selected="True">-</asp:ListItem>
	                <asp:ListItem>OR</asp:ListItem>
	                <asp:ListItem>AND</asp:ListItem>	                
	            </asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlField1" Width="150px" Enabled ="false" runat="server"></asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlNotasi1" Enabled ="false" runat="server">
	                <asp:ListItem Selected="True">=</asp:ListItem>
	                <asp:ListItem>>=</asp:ListItem>
	                <asp:ListItem><=</asp:ListItem>
	                <asp:ListItem>LIKE</asp:ListItem>
	                <asp:ListItem>NOT LIKE</asp:ListItem>
	            </asp:DropDownList>
	            <asp:TextBox ID="tbField1" Enabled ="false" runat="server" />
	        </td>
	    </tr>
	    <tr>
	        <td>
	            <asp:DropDownList ID="ddlOperator2" AutoPostBack="true" runat="server">
	                <asp:ListItem Selected="True">-</asp:ListItem>
	                <asp:ListItem>OR</asp:ListItem>
	                <asp:ListItem>AND</asp:ListItem>	                
	            </asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlField2" Width="150px" Enabled ="false" runat="server"></asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlNotasi2" Enabled ="false" runat="server">
	                <asp:ListItem Selected="True">=</asp:ListItem>
	                <asp:ListItem>>=</asp:ListItem>
	                <asp:ListItem><=</asp:ListItem>
	                <asp:ListItem>LIKE</asp:ListItem>
	                <asp:ListItem>NOT LIKE</asp:ListItem>
	            </asp:DropDownList>
	            <asp:TextBox ID="tbField2" Enabled ="false" runat="server" />
	        </td>
	    </tr>
	    <tr>
	        <td>
	            <asp:DropDownList ID="ddlOperator3" AutoPostBack="true" runat="server">
	                <asp:ListItem Selected="True">-</asp:ListItem>
	                <asp:ListItem>OR</asp:ListItem>
	                <asp:ListItem>AND</asp:ListItem>	                
	            </asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlField3" Width="150px" Enabled ="false" runat="server"></asp:DropDownList>
	        </td>
	        <td>
	            <asp:DropDownList ID="ddlNotasi3" Enabled ="false" runat="server">
	                <asp:ListItem Selected="True">=</asp:ListItem>
	                <asp:ListItem>>=</asp:ListItem>
	                <asp:ListItem><=</asp:ListItem>
	                <asp:ListItem>LIKE</asp:ListItem>
	                <asp:ListItem>NOT LIKE</asp:ListItem>
	            </asp:DropDownList>
	            <asp:TextBox ID="tbField3" Enabled ="false" runat="server" />
	        </td>
	    </tr>
	</table>
	<asp:Button ID="btnSearch" runat="server" Text="Search" />
</fieldset>
<br />
<asp:Label ID="lbStatus" runat="server" ForeColor="Red" />