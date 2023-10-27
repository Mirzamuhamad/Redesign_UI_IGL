<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CategoryReport2.ascx.vb" Inherits="UserControl_CategoryReport2" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>       
        <table width="100%">
            <tr>
                <td>
                    Product Group : <asp:DropDownList ID="ddlGroup" runat="server" CssClass="DropDownList" />
                </td>
            </tr>    
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
                </td>   
                <td>
                    <asp:CheckBox ID="cbGrid2" AutoPostBack="true" runat="server"/>
                    <asp:DropDownList CssClass="DropDownList" AutoPostBack="true" id="DDLGrid2" runat="server" />  
                </td>           
                
            </tr>
            <tr>
                <td style="width:47%;">
                   <div style="border:0px  solid; width:100%; height:300px; overflow:auto;"> 
                   <dx:ASPxGridView ID="Grid1" ClientInstanceName="Grid1" runat="server" Width="100%"  style="table-layout:fixed;" KeyFieldName="CODE" EmptyDataText="There are no data record(s) to display."
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
                            <dx:GridViewDataColumn FieldName="CODE" Caption="Code" VisibleIndex="1" />
                            <dx:GridViewDataColumn FieldName="Description" Caption="Description" VisibleIndex="2" />
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
                <td style="width:6%">&nbsp</td>
                <td style="width:47%;">
                  <div style="border:0px  solid; width:100%; height:300px; overflow:auto;">
                  <dx:ASPxGridView ID="Grid2" ClientInstanceName="Grid2" runat="server" Width="100%"  style="table-layout:fixed;" KeyFieldName="CODE"
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
                            <dx:GridViewDataColumn FieldName="CODE" Caption="Code" VisibleIndex="1" />
                            <dx:GridViewDataColumn FieldName="Description" Caption="Description" VisibleIndex="2" />
                        </Columns>
                    </dx:ASPxGridView>
                    <%--<asp:GridView ID="Grid2" Width="100%" runat="server"
                        EmptyDataText="There are no data records to display." AutoGenerateColumns="False"
                        DataSourceID="dsGrid2">
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
	                    <RowStyle CssClass="GridItem" />	
	                    <AlternatingRowStyle CssClass="GridAltItem"/>
	                    <PagerStyle CssClass="GridPager" /> 
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect2" runat="server" />
                                </ItemTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <td>Code</td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBox" />
                                                <asp:Button ID="btnCode" runat="server" CommandName="Filter" CssClass="Button" Text="..." />
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                </ItemTemplate>                            
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <td>Name</td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox ID="tbName" runat="server" CssClass="TextBox" />
                                                <asp:Button ID="btnName" runat="server" CommandName="Filter" CssClass="Button" Text="..." />
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbName" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                </ItemTemplate>                             
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                  </div>  
                    <asp:SqlDataSource ID="dsGrid2" runat="server">
                    </asp:SqlDataSource> 
                    <%--ConnectionString="<%$ ConnectionStrings:IKKJConnectionString %>" 
                        ProviderName="System.Data.SqlClient"--%>
                </td>     
            </tr>
          </table>
<asp:Label ID="lStatus" runat="server" ForeColor="Red" />       