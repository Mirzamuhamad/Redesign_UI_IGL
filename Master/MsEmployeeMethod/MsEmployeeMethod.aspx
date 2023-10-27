<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsEmployeeMethod.aspx.vb" Inherits="Master_MsEmployeeMethod_MsEmployeeMethod" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lbjudul"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Value="Emp_No" Text="Employee ID"></asp:ListItem>
                    <asp:ListItem Value="Emp_Name" Text="Employee Name"></asp:ListItem>                    
                    <asp:ListItem Value="Gender" Text="Gender"></asp:ListItem>
                    <asp:ListItem Value="JobTitle_Name" Text="Job Title"></asp:ListItem>
                    <asp:ListItem Value="JobLevel_Name" Text="Job Level"></asp:ListItem>
                    <asp:ListItem Value="EmpStatus_Name" Text="Employee Status"></asp:ListItem>
                    <asp:ListItem Value="Work_Place_Name" Text="Work Place"></asp:ListItem>                    
                    <%--<asp:ListItem Value="Sub_Section_Name" Text="Sub Section"></asp:ListItem>                    
                    <asp:ListItem Value="Section_Name" Text="Section"></asp:ListItem>                    --%>
                    <asp:ListItem Value="Dept_Name" Text="Organization"></asp:ListItem>                    
                    <asp:ListItem Value="MethodSalary_Name" Text="Method Salary"></asp:ListItem>                    
                    <asp:ListItem Value="MethodTHR_Name" Text="Method THR"></asp:ListItem>                    
                    <asp:ListItem Value="FgActive" Text="Active"></asp:ListItem>                    
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <%--<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>--%>
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="lb1" runat="server" Text = "Setting Method in selected row : "> </asp:Label>
                <asp:DropDownList ID="ddlMethod" runat="server" CssClass="DropDownList" Width="180px" >                    
                </asp:DropDownList>     
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply" Text="G"/>
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
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Value="Emp_No" Text="Employee ID"></asp:ListItem>
                    <asp:ListItem Value="Emp_Name" Text="Employee Name"></asp:ListItem>                    
                    <asp:ListItem Value="Gender" Text="Gender"></asp:ListItem>
                    <asp:ListItem Value="JobTitle_Name" Text="Job Title"></asp:ListItem>
                    <asp:ListItem Value="JobLevel_Name" Text="Job Level"></asp:ListItem>
                    <asp:ListItem Value="EmpStatus_Name" Text="Employee Status"></asp:ListItem>
                    <asp:ListItem Value="Work_Place_Name" Text="Work Place"></asp:ListItem>                    
                    <%--<asp:ListItem Value="Sub_Section_Name" Text="Sub Section"></asp:ListItem>                    
                    <asp:ListItem Value="Section_Name" Text="Section"></asp:ListItem>  --%>                  
                    <asp:ListItem Value="Dept_Name" Text="Organization"></asp:ListItem>                    
                    <asp:ListItem Value="MethodSalary_Name" Text="Method Salary"></asp:ListItem>                    
                    <asp:ListItem Value="MethodTHR_Name" Text="Method THR"></asp:ListItem>                    
                    <asp:ListItem Value="FgActive" Text="Active"></asp:ListItem>                    
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				          <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                        oncheckedchanged="cbSelectHd_CheckedChanged1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>                            
							<asp:TemplateField HeaderText="Employee ID" HeaderStyle-Width="100" SortExpression="Emp_No">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpNumb" text='<%# DataBinder.Eval(Container.DataItem, "Emp_No") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="EmpNumbEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Emp_No") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="320" SortExpression="Emp_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpName" text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" CssClass="TextBox" ID="EmpNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>									
								</EditItemTemplate>
                                <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>								
							<asp:TemplateField HeaderText="Job Title" HeaderStyle-Width="180" SortExpression="JobTitle_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobTitle" text='<%# DataBinder.Eval(Container.DataItem, "JobTitle_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="JobTitleEdit" Text='<%# DataBinder.Eval(Container.DataItem, "JobTitle_Name") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <HeaderStyle Width="180px"></HeaderStyle>
							</asp:TemplateField>							
							<asp:TemplateField HeaderText="Organization" HeaderStyle-Width="180" SortExpression="Dept_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Department" text='<%# DataBinder.Eval(Container.DataItem, "Dept_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DepartmentEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Dept_Name") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <HeaderStyle Width="180px"></HeaderStyle>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Work Place" HeaderStyle-Width="180" SortExpression="Work_Place_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkPlace" text='<%# DataBinder.Eval(Container.DataItem, "Work_Place_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WorkPlaceEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Work_Place_Name") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <HeaderStyle Width="180px"></HeaderStyle>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Method Salary" SortExpression="MethodSalary_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodSalary" text='<%# DataBinder.Eval(Container.DataItem, "MethodSalary_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" Width="100%" ID="MethodSalaryEdit" 
									     SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MethodSalary") %>' 
                                        DataSourceID="dsMethodSalary" DataTextField="MethodName" 
                                        DataValueField="MethodCode">									    
									</asp:DropDownList>	
								</EditItemTemplate>								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Method THR" SortExpression="MethodTHR_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodTHR" text='<%# DataBinder.Eval(Container.DataItem, "MethodTHR_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" Width="100%" ID="MethodTHREdit" 
									    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MethodTHR") %>' 
                                        DataSourceID="dsMethodTHR" DataTextField="MethodName" 
                                        DataValueField="MethodCode">									    
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>															
                                <HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsMethodSalary" runat="server" SelectCommand="EXEC S_GetMethodSalaryChoose">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsMethodTHR" runat="server" SelectCommand="EXEC S_GetMethodTHRChoose">
        </asp:SqlDataSource>

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
