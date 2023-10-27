<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProcessPremiHadir.Aspx.vb" Inherits="Master_MsProcessPremiHadir_MsProcessPremiHadir" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Price File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Process BAP Panen & Premi</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Week" Value="Week"></asp:ListItem>
                  <asp:ListItem Text="WeekNo" Value="Week No"></asp:ListItem>                            
                  <asp:ListItem Text="DonePremi" Value="Premi"></asp:ListItem>                            
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
               <%-- <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        --%>
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                     <asp:ListItem Selected="true" Text="Week" Value="Week"></asp:ListItem>
                  <asp:ListItem Text="WeekNo" Value="Week No"></asp:ListItem>                            
                  <asp:ListItem Text="DonePremi" Value="Premi"></asp:ListItem>                                           
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlYear" CssClass="DropDownList" AutoPostBack= "true" >                            
             </asp:DropDownList>
            </td>
            <td>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Process" />
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnUnApply" Text="UnProcess" />
           
            </td>
             <td style="width:100px;text-align:right" ></td>
            <td> </td>
        </tr>
    </table> 
    <br />
    <asp:GridView id="DataGrid" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField>
                                  <HeaderTemplate>
                                      <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                                      oncheckedchanged="cbSelectHd_CheckedChanged" />
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:CheckBox ID="cbSelect" runat="server" />
                                  </ItemTemplate>
                              </asp:TemplateField>
				            <asp:TemplateField HeaderText="Week" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Week" text='<%# DataBinder.Eval(Container.DataItem, "Week") %>'/>
									<asp:Label Runat="server" ID="WeekNo" Visible="false"  text='<%# DataBinder.Eval(Container.DataItem, "WeekNo") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Start Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="tbStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="StartDate" DataFormatString="{0:dd MMM yyyy}"
                                HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>								
						
							<asp:TemplateField HeaderText="End Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="tbEndDate" Text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EndDate" DataFormatString="{0:dd MMM yyyy}"
                                HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
							
							<asp:TemplateField HeaderText="Process Premi" ItemStyle-HorizontalAlign = "Left" SortExpression="DonePremi">
								<Itemtemplate>
								<asp:Label runat="server" ID="DonePremi" text='<%# DataBinder.Eval(Container.DataItem, "DonePremi") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
												
    					</Columns>
        </asp:GridView>               
    </div>
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>     
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
