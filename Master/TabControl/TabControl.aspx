<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabControl.aspx.cs" Title="AJAXRND ::: Tab Control Implementation" Inherits="TabControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        &nbsp;&nbsp;
        <table style="width: 100%">
            <tr>
                <td>
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <cc1:tabcontainer id="TabContainer1" runat="server" activetabindex="1" AutoPostBack="True" OnActiveTabChanged="TabContainer1_ActiveTabChanged"><cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1"><HeaderTemplate>
Student Details
</HeaderTemplate>
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="stdnt_cd" HeaderText="Student ID" />
                        <asp:BoundField DataField="rgstrtn_cd" HeaderText="Registration No." />
                        <asp:BoundField DataField="sbjct_chsn" HeaderText="Subject Status" />
                        <asp:BoundField DataField="stdnt_stts" HeaderText="Student Status" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
                &nbsp;<br />
            </ContentTemplate>
</cc1:TabPanel>
<cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2"><HeaderTemplate>
Batch&nbsp;Details
</HeaderTemplate>
    <ContentTemplate>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="btch_cd" HeaderText="Batch Code" />
                <asp:BoundField DataField="smstr_cd" HeaderText="Semester Code" />
                <asp:BoundField DataField="smstr_vrsn" HeaderText="Semester Version" />
                <asp:BoundField DataField="mx_nmbr_stdnt" HeaderText="Maximum No. of Student" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
    </ContentTemplate>
</cc1:TabPanel>
<cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="TabPanel3"><HeaderTemplate>
    Course Details&nbsp;
</HeaderTemplate>
    <ContentTemplate>
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="crs_ttl" HeaderText="Course Title" />
                <asp:BoundField DataField="crs_drtn" HeaderText="Course Duration" />
                <asp:BoundField DataField="smrt_crd_rqrd" HeaderText="Smart Card Required" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
    </ContentTemplate>
</cc1:TabPanel>
</cc1:tabcontainer><br />                
                <br />
                &nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                    <ProgressTemplate>
                        &nbsp;<img src="ajax-loader.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </td>
            </tr>
        </table>
        <br />
        &nbsp;<div>
            &nbsp;</div>
    </form>
</body>
</html>
