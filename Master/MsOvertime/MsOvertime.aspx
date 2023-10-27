<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsOvertime.aspx.vb" Inherits="Transaction_MsOvertime_MsOvertime" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register assembly="BasicFrame.WebControls.BasicDatePicker" namespace="BasicFrame.WebControls" tagprefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 89px;
        }
        .style2
        {
            width: 358px;
        }
        .style5
        {
            width: 124px;
        }
        .style6
        {
            width: 128px;
        }
        .style7
        {
            width: 167px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Overtime File</div>
     <hr style="color:Blue" />           
     <table>
           <tr>
              <td class="style1">Work Place</td>
              <td>:</td>
              <td><asp:TextBox ID="tbWorkPlaceCode" runat="server" CssClass="TextBoxR" Width="137px" />
                  <asp:TextBox ID="tbWorkPlaceName" runat="server" CssClass="TextBoxR" Width="180px" />
                  <asp:Button class="btngo" runat="server" ID="btnWorkPlace" Text="..."/>     
              </td>
           </tr>
           </table>
     <br />
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
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>
            </Items>            
     </asp:Menu> <br /> 
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
                 
           <asp:View ID="tab1" runat="server">           
           <br />
           
           <asp:Panel runat="server" ID="Panel2">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">                  
                    <asp:GridView ID="DataGrid" runat="server" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Hour Netto" SortExpression="OTHour ">
                                <Itemtemplate>
                                    <asp:Label ID="OTHour" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "OTHour") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="OTHourEdit" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OTHour") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="OTHourAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="OTHourAdd_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="OTHourAdd" WatermarkCssClass="Watermarked" 
                                        WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="OT Hour Working" SortExpression="OTHourWorking">
                                <Itemtemplate>
                                    <asp:Label ID="OTHourWorking" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "OTHourWorking") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="OTHourWorkingEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "OTHourWorking") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="OTHourWorkingEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="OTHourWorkingEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="OTHourWorkingAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="OTHourWorkingAdd_WtExt" runat="server" Enabled="True" TargetControlID="OTHourWorkingAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="OT Hour Holiday" SortExpression="OTHourHoliday">
                                <Itemtemplate>
                                    <asp:Label ID="OTHourHoliday" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "OTHourHoliday") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="OTHourHolidayEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "OTHourHoliday") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="OTHourHolidayEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="OTHourHolidayEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="OTHourHolidayAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="OTHourHolidayAdd_WtExt" runat="server" Enabled="True" TargetControlID="OTHourHolidayAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                                                    
                            <asp:TemplateField HeaderStyle-Width="126" HeaderText="Action">
                                <Itemtemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit" Text="Edit" />
                                    <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" 
                                        CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" CommandName="Update" Text="Save" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" CommandName="Cancel" Text="Cancel" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnAdd" runat="server" class="bitbtndt btnadd" CommandName="Insert" Text="Add" />
                                </FooterTemplate>
                            </asp:TemplateField>                          
                        </Columns>
                    </asp:GridView>
              </div>   
           </asp:Panel> 
           
           </asp:View>   
                   
        </asp:MultiView>
    
      <br />
       
       <%--<asp:SqlDataSource ID="dsAccClass" runat="server" SelectCommand="SELECT DISTINCT Class_Code, Class_Account FROM VMsaccount WHERE FgType = 'PL'">
       </asp:SqlDataSource> --%>
       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>

</html>
