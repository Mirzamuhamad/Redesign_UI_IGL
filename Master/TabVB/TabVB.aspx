<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TabVB.aspx.vb" Inherits="Master_TabVB_TabVB" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Common User File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .BackColorTab
        {
            font-family:Verdana, Arial, Courier New;
            font-size: 10px;
            color:Gray;
            background-color:Transparent;
        }
        .TabHeaderCSS
        {
             font-family:Verdana, Arial, Courier New;
             font-size: 10px;
             background-color: Transparent;
             text-align: center;
             cursor: pointer;
             height:500px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Work Place File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Work Palce Code" Value="WorkPlaceCode"></asp:ListItem>
                  <asp:ListItem Text="Work Place Name" Value="WorkPlaceName"></asp:ListItem>        
                  <asp:ListItem Text="Minimum Wage (Rp)" Value="GajiUMK"></asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" 
                    Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
        </tr>
     </table>
     
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" CssClass="TextBox ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Work Place Code" Value="WorkPlaceCode"></asp:ListItem>
                    <asp:ListItem Text="Work Place Name" Value="WorkPlaceName"></asp:ListItem>
                    <asp:ListItem Text="Minimum Wage (Rp)" Value="GajiUMK"></asp:ListItem> 
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
     <br />
     
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:Button class="bitbtn btnsearch" runat="server" ID="Button1" 
                    Text="1" />
     <asp:Button class="bitbtn btnsearch" runat="server" ID="Button2" 
                    Text="2" />
     <asp:Button class="bitbtn btnsearch" runat="server" ID="Button3" 
                    Text="3" />
     <br />    
        <cc1:tabcontainer id="TabContainer1" CssClass="TabHeaderCSS" runat="server" activetabindex="1" >
         <cc1:TabPanel runat="server" ID="TabPanel1" >
            <%--<HeaderTemplate>View Grid</HeaderTemplate>--%>
            <ContentTemplate>
                <asp:Label runat="server" ID="lb1" Text="Panel 1" ></asp:Label>
                <br />
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
<cc1:TabPanel runat="server" ID="TabPanel2">
    <%--<HeaderTemplate>Batch Details</HeaderTemplate>--%>
    <ContentTemplate>
        <asp:Label runat="server" ID="Label1" Text="Panel 2" ></asp:Label>
                <br />
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
<cc1:TabPanel runat="server" ID="TabPanel3" >
<%--<HeaderTemplate>Course Details</HeaderTemplate>--%>
    <ContentTemplate>
        <asp:Label runat="server" ID="Label2" Text="Panel 3" ></asp:Label>
                <br />
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
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "False"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Work Place Code" HeaderStyle-Width="100" SortExpression="WorkPlaceCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkPlaceCode" text='<%# DataBinder.Eval(Container.DataItem, "WorkPlaceCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WorkPlaceCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WorkPlaceCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WorkPlaceCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WorkPlaceCoeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WorkPlaceCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Work Place Name" HeaderStyle-Width="320" SortExpression="WorkPlaceName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkPlaceName" text='<%# DataBinder.Eval(Container.DataItem, "WorkPlaceName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="WorkPlaceNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "WorkPlaceName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="WorkPlaceNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WorkPlaceNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WorkPlaceNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WorkPlaceNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WorkPlaceNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Minimum Wage (Rp)" HeaderStyle-Width="100" SortExpression="GajiUMK" ItemStyle-HorizontalAlign="Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="GajiUMK" text='<%# DataBinder.Eval(Container.DataItem, "GajiUMK") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="GajiUMKEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "GajiUMK") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="GajiUMKEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="GajiUMKEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="GajiUMKAdd" CssClass="TextBox" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="GajiUMKAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="GajiUMKAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>						
    					</Columns>
        </asp:GridView>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
