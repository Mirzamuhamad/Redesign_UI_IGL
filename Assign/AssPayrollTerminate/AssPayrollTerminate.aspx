<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AssPayrollTerminate.aspx.vb" Inherits="Assign_AssPayrollTerminate_AssPayrollTerminate" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
    <hr style="color:Blue" /> 
        <br />        
        <asp:Panel runat="server" ID="pnlCopy" Visible="False">
              <table width="100%">
                <tr>
                    <td>Group By</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" ID = "ddlGroupByCopy" runat = "server" AutoPostBack="true" >
                            <asp:ListItem Selected="True">PHK Reason</asp:ListItem>
                            <asp:ListItem>Payroll</asp:ListItem>
                        </asp:DropDownList> </td>
                </tr>
                <tr>
                    <td>From</td>
                    <td>:</td>
                    <td>                    
                        <asp:TextBox CssClass="TextBox" Id="tbFromCode" runat="server" Width="75px" AutoPostBack="True" />
                        <asp:TextBox CssClass="TextBox" ID="tbFromName" runat="server" Width="150px" Enabled="False" /> 
                        <asp:Button ID="btnSearchFrom" runat="server" class="btngo" Text="..."/>                        
                    </td>
                </tr>
                <tr>
                    <td>To</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbToCode" runat="server" Width="75px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbToName" runat="server" Width="150px" Enabled="False" /> 
                        <asp:Button ID="btnSearchTo" runat="server" class="btngo" Text="..."/>
                        
                    </td>
                </tr>                
                <tr>
                    <td colspan = "3">
                        <asp:Button class="bitbtn btncopy" runat="server" ID="btnCopy" Text="Copy" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />
                    </td>                                   
                </tr>
                </table>
            </asp:Panel>       
        
        <asp:Panel runat="server" ID="PnlAssign">          
              <table width="100%">
                <tr>
                    <td style="width:100px">Group By </td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlGroupBy" runat = "server" AutoPostBack="true" >
                            <asp:ListItem Selected="True">PHK Reason</asp:ListItem>
                            <asp:ListItem>Payroll</asp:ListItem>
                        </asp:DropDownList>                            
                    
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="75px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="150px" Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/>                        
                    </td>
               </tr>                         
                  <tr>
                      <td style="width:100px">
                          Percentage (%)</td>
                      <td>
                          :</td>
                      <td>
                          <asp:TextBox ID="tbPercentage" runat="server" AutoPostBack="True" 
                              CssClass="TextBox" Width="75px" />
                      </td>
                  </tr>
            </table>
            <br />
            <asp:Button class="bitbtn btncopyto" runat="server" ID="btnCopyTo" Text="Copy To" />
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
                        AllowPaging="True" DataSourceID="dsAssigned" AutoGenerateColumns="False"><Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                        <HeaderTemplate>
                        <input type="checkbox" onclick="AssignedGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" />
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn FieldName="CODE" VisibleIndex="1" >
                            <Settings AutoFilterCondition="Contains" />
                         </dx:GridViewDataColumn>
                         <dx:GridViewDataColumn FieldName="NAME" VisibleIndex="2" >
                        <Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Percentage" VisibleIndex="3" >
                        <Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
                        </Columns><Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" /></dx:ASPxGridView>
                    </div>
                    <asp:SqlDataSource ID="dsAssigned" runat="server" />
                </td>
                <td style="width:3%" align ="center">
                        <asp:Button OnClick="btnAddAll_Click" ID="btnAddAll" ToolTip="Add All"  runat="server" class="btnassign" Text="<<"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnAdd_Click" ID="btnAdd" ToolTip="Add"  runat="server" class="btnassign" Text="<"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnRemove_Click" ID="btnRemove" ToolTip="Remove"  runat="server" class="btnassign" Text=">"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnRemoveAll_Click" ID="btnRemoveAll" ToolTip="Remove All"  runat="server" class="btnassign" Text=">>"/>                                                                                                 
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
