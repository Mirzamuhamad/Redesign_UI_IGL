<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWODownPriority.Aspx.vb" Inherits="Transaction_TrWODownPriority" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Down Priority</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label ID="lbltitle" runat="server" Text="Label"></asp:Label> </div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text=" WO_No" Value="WO_No"></asp:ListItem>
                  <asp:ListItem Text="WO Date" Value="WO_Date"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="JobPlant"></asp:ListItem>                            
                  <asp:ListItem Text="Job Name" Value="Job_Name"></asp:ListItem>                                           
                  <asp:ListItem Text="Divisi" Value="Divisi"></asp:ListItem>
                  <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>
                  <asp:ListItem Text="Divisi Blok" Value="DivisiBlok"></asp:ListItem>                            
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                  <asp:ListItem Text="Btm Week" Value="EstStartWeek"></asp:ListItem>                            
                  <asp:ListItem Text="Top Week" Value="EstEndWeek"></asp:ListItem>                                           
                  <asp:ListItem Text="Work By" Value="WorkBy"></asp:ListItem>
                                                                                           
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
                  <asp:ListItem Selected="true" Text=" WO_No" Value="WO_No"></asp:ListItem>
                  <asp:ListItem Text="WO Date" Value="WO_Date"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="JobPlant"></asp:ListItem>                            
                  <asp:ListItem Text="Job Name" Value="Job_Name"></asp:ListItem>                                           
                  <asp:ListItem Text="Divisi" Value="Divisi"></asp:ListItem>
                  <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>
                  <asp:ListItem Text="Divisi Blok" Value="DivisiBlok"></asp:ListItem>                            
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                  <asp:ListItem Text="Btm Week" Value="EstStartWeek"></asp:ListItem>                            
                  <asp:ListItem Text="Top Week" Value="EstEndWeek"></asp:ListItem>                                           
                  <asp:ListItem Text="Work By" Value="WorkBy"></asp:ListItem>                                             
                  
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <%--<td>Year</td>
            <td>:</td>--%>
            <td><asp:DropDownList runat="server" ID="ddlYear" CssClass="DropDownList" Visible="false"
                    AutoPostBack="True"> </asp:DropDownList> </td>
             <td> &nbsp;</td>
             <td>
              <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Down" 
                     width="85"/>
            <%--<asp:Button class="bitbtn btnapply" runat="server" ID="BtnUnApply" Text="UnComplete" width="85"/>--%>
            </td>
        </tr>
       
    </table> 
    <br />
    
     <asp:Panel ID="pnlClosing" runat="server" Visible="false">
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
				            <%--<asp:TemplateField HeaderText="Closing" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DoneClosing" text='<%# DataBinder.Eval(Container.DataItem, "DoneClosing") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>--%>
				           
							<asp:TemplateField HeaderText="WoNo" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Reference" text='<%# DataBinder.Eval(Container.DataItem, "WO_No") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="WO Date" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WODate" text='<%# DataBinder.Eval(Container.DataItem, "WO_Date") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:BoundField DataField="WO_Date" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="WODate" HeaderText="WO Date">
                            <HeaderStyle Width="80px" /> </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Job Plan Code" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobPlanCode" text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
                            
							<asp:TemplateField HeaderText="Job Plan Name" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>'/>
                                    <asp:Label Runat="server" ID="Job_Name" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:BoundField DataField="Qty"  DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" SortExpression="Qty" HeaderText="Qty">
                            <HeaderStyle Width="80px" /> 
                            </asp:BoundField>
							
							<asp:TemplateField HeaderText="Unit" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>								            
							
				            <asp:TemplateField HeaderText="Divisi Code" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisiCode" text='<%# DataBinder.Eval(Container.DataItem, "Divisi") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Divisi Name" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisiName" text='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Btm Week" >
								<Itemtemplate>
									<asp:Label runat="server" ID="BtmWeek" text='<%# DataBinder.Eval(Container.DataItem, "EstStartWeek") %>' />
									<%--<asp:Label runat="server" ID="DivisiBlok_Name" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok_Name") %>' />--%>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Top Week" >
								<Itemtemplate>
									<asp:Label runat="server" ID="TopWeek" text='<%# DataBinder.Eval(Container.DataItem, "EstEndWeek") %>' />
									<%--<asp:Label runat="server" ID="DivisiBlok_Name" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok_Name") %>' />--%>
								</Itemtemplate>														
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Work By" >
								<Itemtemplate>
									<asp:Label runat="server" ID="WorkBy" text='<%# DataBinder.Eval(Container.DataItem, "WorkBy ") %>' />
									<%--<asp:Label runat="server" ID="DivisiBlok_Name" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok_Name") %>' />--%>
								</Itemtemplate>														
							</asp:TemplateField>
							
							
														
							
												
    					</Columns>
        </asp:GridView>               
      </asp:Panel>  
        
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
