<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductCapacity.aspx.vb" Inherits="Transaction_MsProductCapacity_MsProductCapacity" %>
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
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />  
     <asp:Panel runat="server" ID="pnlCopy" Visible="False">
              <table width="100%">                
                <tr>
                    <td>From</td>
                    <td>:</td>
                    <td>                    
                        <asp:TextBox CssClass="TextBox" Id="tbFromCode" runat="server" AutoPostBack="True" />
                        <asp:TextBox CssClass="TextBox" ID="tbFromName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearchFrom" runat="server" class="btngo" Text="..."/>                        
                    </td>
                </tr>
                <tr>
                    <td>To</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbToCode" runat="server" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbToName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearchTo" runat="server" class="btngo" Text="..."/>
                        
                    </td>
                </tr>                
                <tr>
                    <td colspan = "3">
                        <asp:Button class="bitbtn btncopy" runat="server" ID="btnCopy" Text="Copy" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />
                        <%--<asp:ImageButton ID="btnCopy" runat="server"  
                            ImageUrl="../../Image/btncopyon.png"
                            onmouseover="this.src='../../Image/btncopyoff.png';"
                            onmouseout="this.src='../../Image/btncopyon.png';"
                            ImageAlign="AbsBottom" />                
                        <asp:ImageButton ID="btnCancel" runat="server"  
                            ImageUrl="../../Image/btnbackon.png"
                            onmouseover="this.src='../../Image/btnbackoff.png';"
                            onmouseout="this.src='../../Image/btnbackon.png';"
                            ImageAlign="AbsBottom" />   --%>             
                    </td>                                   
                </tr>
                </table>
            </asp:Panel>  
            <asp:Panel runat="server" ID="pnlAssign">       
            <table>
            <tr>
              <td class="style1">Product</td>
              <td>:</td>
              <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbProductCode" AutoPostBack="True"/> 
                  <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" Enabled = False Width="200px" />
                  <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..."/>                    
                  <asp:Button class="bitbtn btncopyto" runat="server" ID="btnCopyTo" Text="Copy To" />   
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
                        
                            
							<asp:TemplateField HeaderText="Machine Group" HeaderStyle-Width="150" SortExpression="MachineGroup_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="MachineGroupEdit" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'>
									</asp:Label>								
									
								</EditItemTemplate>
								<FooterTemplate>								
									<asp:TextBox ID="MachineGroupAdd" CssClass="TextBox" MaxLength="5" Width="70%" 
									ontextchanged="MachineGroupAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="MachineGroupAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGroupAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>			
                                    <asp:Button class="btngo" runat="server" ID="btnMachineGroupAdd" CommandName ="btnMachineGroupAdd" Text="..."  />																						
								</FooterTemplate>															
							</asp:TemplateField>							  			
							
							<asp:TemplateField HeaderText="Machine Group Name" HeaderStyle-Width="200" SortExpression="MachineGroup_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGroupName" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Name") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineGroupNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Name") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="MachineGroupNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Capacity Qty" SortExpression="CapacityQty">
                                <Itemtemplate>
                                    <asp:Label ID="CapacityQty" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CapacityQty") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="CapacityQtyEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CapacityQty") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="CapacityQtyEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="CapacityQtyEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="CapacityQtyAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="CapacityQtyAdd_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="CapacityQtyAdd" WatermarkCssClass="Watermarked" 
                                        WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Capacity Machine (Hours)" SortExpression="CapacityHour">
                                <Itemtemplate>
                                    <asp:Label ID="CapacityHour" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CapacityHour") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="CapacityHourEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CapacityHour") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="CapacityHourEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="CapacityHourEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="CapacityHourAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="CapacityHourAdd_WtExt" runat="server" Enabled="True" TargetControlID="CapacityHourAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Cycle Time" SortExpression="CycleTime">
                                <Itemtemplate>
                                    <asp:Label ID="CycleTime" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CycleTime") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="CycleTimeEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CycleTime") %>' Width="100%">
                                    </asp:TextBox>
                                    <%--<cc1:TextBoxWatermarkExtender ID="CycleTimeEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="CycleTimeEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="CycleTimeAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <%--<cc1:TextBoxWatermarkExtender ID="CycleTimeAdd_WtExt" runat="server" Enabled="True" TargetControlID="CycleTimeAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Setting Time" SortExpression="SettingTime">
                                <Itemtemplate>
                                    <asp:Label ID="SettingTime" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SettingTime") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="SettingTimeEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "SettingTime") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="SettingTimeEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="SettingTimeEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="SettingTimeAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="SettingTimeAdd_WtExt" runat="server" Enabled="True" TargetControlID="SettingTimeAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Max Machine Run" SortExpression="MaxMachine">
                                <Itemtemplate>
                                    <asp:Label ID="MaxMachine" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "MaxMachine") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="MaxMachineEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MaxMachine") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="MaxMachineEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="MaxMachineEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="MaxMachineAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="MaxMachineAdd_WtExt" runat="server" Enabled="True" TargetControlID="MaxMachineAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Cycle Time / Man" SortExpression="CycleTimeMan">
                                <Itemtemplate>
                                    <asp:Label ID="CycleTimeMan" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CycleTimeMan") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="CycleTimeManEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CycleTimeMan") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="CycleTimeManEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="CycleTimeManEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="CycleTimeManAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="CycleTimeManAdd_WtExt" runat="server" Enabled="True" TargetControlID="CycleTimeManAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Man Power" SortExpression="ManPower">
                                <Itemtemplate>
                                    <asp:Label ID="ManPower" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ManPower") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:TextBox ID="ManPowerEdit" Runat="server" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ManPower") %>' Width="100%">
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="ManPowerEdit_WtExt" runat="server" 
                                        Enabled="True" TargetControlID="ManPowerEdit" 
                                        WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ManPowerAdd" Runat="Server" CssClass="TextBox" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="ManPowerAdd_WtExt" runat="server" Enabled="True" TargetControlID="ManPowerAdd" WatermarkCssClass="Watermarked" WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>   
                            
                            <asp:TemplateField HeaderText="Fg Default" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgDefault">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgDefault" text='<%# DataBinder.Eval(Container.DataItem, "FgDefault") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgDefaultEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgDefault") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgDefaultAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
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
    </asp:Panel>
      <br />
       
       <%--<asp:SqlDataSource ID="dsAccClass" runat="server" SelectCommand="SELECT DISTINCT Class_Code, Class_Account FROM VMsaccount WHERE FgType = 'PL'">
       </asp:SqlDataSource> --%>
       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>

</html>
