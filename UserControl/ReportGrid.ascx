<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReportGrid.ascx.vb" Inherits="ReportGrid" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
        
<table width="100%">
            <tr>
                <td>
                    <asp:CheckBox ID="cbGrid1" runat="server" AutoPostBack="true"/>
                    <asp:DropDownList CssClass="DropDownList" AutoPostBack="true" id="DDLGrid1" runat="server" />                
                </td>                             
                <td>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlNotasi" runat="server">
                        <asp:ListItem>AND</asp:ListItem>
                        <asp:ListItem>OR</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbGrid2" AutoPostBack="true" runat="server"/>
                    <asp:DropDownList CssClass="DropDownList" AutoPostBack="true" id="DDLGrid2" runat="server" />  
                </td>
            </tr>
            <tr>
                <td style="width:47%;">
                   <div style="border:0px  solid; width:96%; height:370px; overflow:auto;">   
                   <dx:ASPxGridView ID="Grid1" Width="100%" ClientInstanceName="Grid1" runat="server" style="table-layout:fixed;" KeyFieldName="CODE"
                            AllowPaging="True" DataSourceID="dsGrid1"> 
                        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                        <Columns>          
                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" >
                                <HeaderTemplate>
                                    <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                        ClientSideEvents-CheckedChanged="function(s, e) { Grid1.SelectAllRowsOnPage(s.GetChecked()); }" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />                            
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataColumn FieldName="CODE" Caption="Code" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="Description" Caption="Description" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    </div>
                    <asp:SqlDataSource ID="dsGrid1" runat="server"></asp:SqlDataSource>
                </td>                         
                <td style="width:47%;">
                  <div style="border:0px  solid; width:96%; height:370px; overflow:auto;">
                  <dx:ASPxGridView ID="Grid2" Width="100%" ClientInstanceName="Grid2" runat="server" style="table-layout:fixed;" KeyFieldName="CODE"
                            AllowPaging="True" DataSourceID="dsGrid2"> 
                        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                        <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" >
                                <HeaderTemplate>
                                    <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ToolTip="Select/Unselect all rows on the page"
                                        ClientSideEvents-CheckedChanged="function(s, e) { Grid2.SelectAllRowsOnPage(s.GetChecked()); }" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />                            
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataColumn FieldName="CODE" Caption="Code" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="Description" Caption="Description" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>                    
                  </div>  
                    <asp:SqlDataSource ID="dsGrid2" runat="server">
                    </asp:SqlDataSource>                     
                </td>
            </tr>
          </table>
<asp:Label ID="lStatus" runat="server" ForeColor="Red" />          