<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AssCustProduct.aspx.vb" Inherits="Assign_AssCustProduct_AssCustProduct" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Product</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>    
    <form id="form1" runat="server">
    <div class="Content">   
    <div class="H1">Assign Customer Product</div>
    <hr style="color:Blue" /> 
        <br />        
        <asp:Panel runat="server" ID="pnlCopy" Visible="False">
              <table width="100%">
                <tr>
                    <td>Group By</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" ID = "ddlGroupByCopy" runat = "server" AutoPostBack="true" >
                            <asp:ListItem Selected="True">Customer</asp:ListItem>
                            <asp:ListItem>Product</asp:ListItem>
                        </asp:DropDownList> </td>
                </tr>
                <tr>
                    <td>From</td>
                    <td>:</td>
                    <td>                    
                        <asp:TextBox Id="tbFromCode" CssClass="TextBox" runat="server" Width="75px" AutoPostBack="True" />
                        <asp:TextBox ID="tbFromName" CssClass="TextBox" runat="server" Width="350px" 
                            Enabled="False" /> 
                        <%--<asp:Button CssClass="Button" ID="btnSearchFrom" runat="server" Text="Search"/>--%>
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnSearchFrom" Text="..." />
                        
                    </td>
                </tr>
                <tr>
                    <td>To</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox Id="tbToCode" CssClass="TextBox" runat="server" Width="75px" AutoPostBack="True"/>
                        <asp:TextBox ID="tbToName" CssClass="TextBox" runat="server" Width="350px" 
                            Enabled="False" /> 
                        <%--<asp:Button CssClass="Button" ID="btnSearchTo" runat="server" Text="Search"/>--%>
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnSearchTo" Text="..." />
                        
                    </td>
                </tr>                
                <tr>
                    <td colspan = "3">
                        <%--<asp:Button CssClass="Button" ID="btnCopy" runat="server" Text="Copy" />--%>
                        <asp:Button class="bitbtn btncopy" runat="server" ID="btnCopy" Text="Copy To" />
                        <%--<asp:Button CssClass="Button" ID="btnCancel" runat="server" Text="Back" />--%>
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />
                        
                    </td>                                   
                </tr>
                </table>
            </asp:Panel>       
        
        <asp:Panel runat="server" ID="PnlAssign">          
              <table width="100%">
                <tr>
                    <td style="width:100px">Group By :</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlGroupBy" runat = "server" AutoPostBack="true" >
                            <asp:ListItem Selected="True">Customer</asp:ListItem>
                            <asp:ListItem>Product</asp:ListItem>
                        </asp:DropDownList>                            
                    
                        <asp:TextBox Id="tbCode" CssClass="TextBox" runat="server" Width="75px" AutoPostBack="True"/>
                        <asp:TextBox ID="tbName" CssClass="TextBox" runat="server" Width="350px" 
                            Enabled="False" /> 
                        <%--<asp:Button CssClass="Button" ID="btnSearch" runat="server" Text="Search"/> --%>
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnsearch" Text="..." />
                        
                    </td>
               </tr>                         
            </table>
            <br />
            <%--<asp:Button ID="btnCopyTo" runat="server" Text="Copy To" CssClass="Button" Height="25px"/>    --%>
            <asp:Button class="bitbtn btncopy" runat="server" ID="btnCopyTo" Text="Copy To" />
                      
            <%--<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="Button" Height="25px" />    --%>
             <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />
            <br />
            
            <table width="100%">
            <tr>
                <td align="center">
                <asp:Label ID="lblAssign" runat="server" Text="Assigned"></asp:Label>
                </td>
                <td>&nbsp</td>
                <td align="center">
                <asp:Label ID="lblAvailable" runat="server" Text="Available"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:40%;">
                   <div style="border:0px  solid; width:100%; height:300px; overflow:auto;">                   
                    <dx:ASPxGridView ID="AssignedGrid" runat="server" Width="100%" style="table-layout:fixed;"
                        EmptyDataText="There are no data records to display." KeyFieldName = "CODE"                     
                        AllowPaging="True" DataSourceID="dsAssigned" AutoGenerateColumns="False"><Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0"><HeaderTemplate><input type="checkbox" onclick="AssignedGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate><HeaderStyle HorizontalAlign="Center" /></dx:GridViewCommandColumn><dx:GridViewDataColumn FieldName="CODE" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn><dx:GridViewDataColumn FieldName="NAME" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn></Columns><Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" /></dx:ASPxGridView>
                    </div>
                    <asp:SqlDataSource ID="dsAssigned" runat="server" />
                        
                </td>
                <td style="width:3%" align="center">
                        <asp:Button class="btnassign" runat="server" OnClick="btnAddAll_Click" ID="btnAddAll" Text="<<" ToolTip="Add All" />
                        <asp:Button class="btnassign" runat="server" OnClick="btnAdd_Click" ID="btnAdd" Text="<" ToolTip="Add" />                        
                        <asp:Button class="btnassign" runat="server" OnClick="btnRemove_Click" ID="btnRemove" Text=">" ToolTip="Remove" />                                                
                        <asp:Button class="btnassign" runat="server" OnClick="btnRemoveAll_Click" ID="btnRemoveAll" Text=">>" ToolTip="Remove All" />                                                                        
                    </td>
                <td style="width:40%;">
                  <div style="border:0px  solid; width:100%; height:300px; overflow:auto;">
                    <dx:ASPxGridView ID="AvailableGrid" runat="server" Width="100%" style="table-layout:fixed;"
                        EmptyDataText="There are no data records to display." KeyFieldName = "CODE"                     
                        AllowPaging="True" DataSourceID="dsAvailable" AutoGenerateColumns="False"><Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0"><HeaderTemplate><input type="checkbox" onclick="AvailableGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate><HeaderStyle HorizontalAlign="Center" /></dx:GridViewCommandColumn><dx:GridViewDataColumn FieldName="CODE" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn><dx:GridViewDataColumn FieldName="NAME" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn></Columns><Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" /></dx:ASPxGridView>
                  </div>  
                    <asp:SqlDataSource ID="dsAvailable" runat="server" />
                </td>
            </tr>
          </table>
        </asp:Panel>
    </div>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>