<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReportSingleGrid.ascx.vb" Inherits="UserControl_ReportSingleGrid" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>   
<table width="50%">
            <tr>
                <td>
                    <asp:CheckBox ID="cbGrid1" runat="server" AutoPostBack="true"/>
                    <asp:DropDownList CssClass="DropDownList" AutoPostBack="true" id="DDLGrid1" runat="server" />                
                </td>             
                
            </tr>
            <tr>
                <td style="width:47%;">
                   <div style="border:0px  solid; width:100%; height:370px; overflow:auto;">  
                   <dx:ASPxGridView ID="Grid1" Width="100%" ClientInstanceName="Grid1" runat="server" KeyFieldName="CODE" EmptyDataText="There are no data record(s) to display."
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
                    <%--<asp:GridView ID="Grid1" Width="100%" runat="server" 
                        EmptyDataText="There are no data records to display." AutoGenerateColumns="False" 
                        DataSourceID="dsGrid1">
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
	                    <RowStyle CssClass="GridItem" />	
	                    <AlternatingRowStyle CssClass="GridAltItem"/>
	                    <PagerStyle CssClass="GridPager" />   
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect1" runat="server" />
                                </ItemTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lbCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                </ItemTemplate>                            
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbName" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                </ItemTemplate>                             
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>--%>
                    </div>
                    <asp:SqlDataSource ID="dsGrid1" runat="server" />
                </td>       
            </tr>
          </table>
<asp:Label ID="lStatus" runat="server" ForeColor="Red" />          