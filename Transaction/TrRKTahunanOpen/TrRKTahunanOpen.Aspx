<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrRKTahunanOpen.Aspx.vb" Inherits="Transaction_TrRKTahunanOpen_TrRKTahunanOpen" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RK Tahunan Open</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
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
                  <asp:ListItem Selected="true" Text="Reference" Value="Reference"></asp:ListItem>
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="Job"></asp:ListItem>                            
                  <asp:ListItem Text="Job_Name" Value="Job Name"></asp:ListItem>                                           
                  <asp:ListItem Text="Division" Value="DivisionName"></asp:ListItem>
                  <asp:ListItem Text="DivisiBlok" Value="Job"></asp:ListItem>                            
                  <asp:ListItem Text="DivisiBlok Nane" Value="DivisiBlok_Name"></asp:ListItem>
                  <asp:ListItem Text="Rotasi" Value="Rotasi"></asp:ListItem>
                  <asp:ListItem Text="StartDate" Value="StartDate"></asp:ListItem>                            
                  <asp:ListItem Text="EndDate" Value="EndDate"></asp:ListItem>                                           
                  <asp:ListItem Text="Team" Value="Team"></asp:ListItem>
                  <asp:ListItem Text="Person" Value="Person"></asp:ListItem>                            
                  <asp:ListItem Text="Capacity" Value="Capacity"></asp:ListItem>                                             
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>                                             
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                                                         
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
                  <asp:ListItem Selected="true" Text="Reference" Value="Reference"></asp:ListItem>
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="Job"></asp:ListItem>                            
                  <asp:ListItem Text="Job_Name" Value="Job Name"></asp:ListItem>                                           
                  <asp:ListItem Text="Division" Value="DivisionName"></asp:ListItem>
                  <asp:ListItem Text="DivisiBlok" Value="Job"></asp:ListItem>                            
                  <asp:ListItem Text="DivisiBlok Nane" Value="DivisiBlok_Name"></asp:ListItem>
                  <asp:ListItem Text="Rotasi" Value="Rotasi"></asp:ListItem>
                  <asp:ListItem Text="StartDate" Value="StartDate"></asp:ListItem>                            
                  <asp:ListItem Text="EndDate" Value="EndDate"></asp:ListItem>                                           
                  <asp:ListItem Text="Team" Value="Team"></asp:ListItem>
                  <asp:ListItem Text="Person" Value="Person"></asp:ListItem>                            
                  <asp:ListItem Text="Capacity" Value="Capacity"></asp:ListItem>                                             
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>                                             
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                             
                  
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
            <td><asp:DropDownList runat="server" ID="ddlYear" CssClass="DropDownList" 
                    AutoPostBack="True"> </asp:DropDownList> </td>
             <td> &nbsp;</td>
             <td>
              <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Open" 
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
				            <asp:TemplateField HeaderText="Closing" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DoneClosing" text='<%# DataBinder.Eval(Container.DataItem, "DoneClosing") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Type" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Reference" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Reference" text='<%# DataBinder.Eval(Container.DataItem, "Reference") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Job" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'/>
                                    <asp:Label Runat="server" ID="Job_Name" text='<%# DataBinder.Eval(Container.DataItem, "Job_Name") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>
							
				            <asp:TemplateField HeaderText="Division" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisionName" text='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="DivisiBlok" >
								<Itemtemplate>
									<asp:Label runat="server" ID="DivisiBlok" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok") %>' />
									<asp:Label runat="server" ID="DivisiBlok_Name" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok_Name") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rotasi" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Rotasi"  text='<%# DataBinder.Eval(Container.DataItem, "Rotasi") %>'/>
									
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Start Date" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:BoundField DataField="StartDate" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date">
                            <HeaderStyle Width="80px" /> </asp:BoundField>
							
							<asp:TemplateField HeaderText="End Date" Visible="false">
								<Itemtemplate >
									<asp:Label Runat="server" ID="EndDate"  text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:BoundField DataField="EndDate" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date">
                            <HeaderStyle Width="80px" /> </asp:BoundField>
							
							<asp:TemplateField HeaderText="Team" >
								<Itemtemplate>
									<asp:Label runat="server" ID="Team" text='<%# DataBinder.Eval(Container.DataItem, "Team") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Person" >
								<Itemtemplate>
									<asp:Label runat="server" ID="Person" text='<%# DataBinder.Eval(Container.DataItem, "Person")%>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
						    <asp:TemplateField HeaderText="Capacity" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Capacity"  text='<%# Format(DataBinder.Eval(Container.DataItem, "Capacity"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							 <asp:TemplateField HeaderText="Qty" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Qty"  text='<%# Format (DataBinder.Eval(Container.DataItem, "Qty"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit" >
								<Itemtemplate>
									<asp:Label runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit")%>' />
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
