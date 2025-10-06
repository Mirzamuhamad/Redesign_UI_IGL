<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsJobCapacity.Aspx.vb" Inherits="Master_MsJobCapacity_MsJobCapacity" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Capacity File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Job Capacity File</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="B.PlantPeriodName"></asp:ListItem>                          
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
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
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="B.PlantPeriodName"></asp:ListItem>                      
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <%--<td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    
            </td>--%>
            
            <td>Job Group</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlJobGroup" CssClass="DropDownList" AutoPostBack="true">                        
                </asp:DropDownList></td>
            <td>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
            </td>
        </tr>
    </table> 
    <br />
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Job Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobCode" text='<%# DataBinder.Eval(Container.DataItem, "JobCode") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CustomerName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Status Tanam">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StatusTanam" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanam") %>' Width="100" Visible="True" />
									<asp:Label Runat="server" ID="StatusTanamName" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanamName") %>' Width="100" />
								</Itemtemplate>																
							</asp:TemplateField>							
						
							
							
							<asp:TemplateField HeaderText="Capacity" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Capacity" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "Capacity") %>' Width="40" />
								<asp:Label Runat="server" ID="lbUnitCap" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								<asp:Label Runat="server" ID="Label2" text="/HK"/>
                                </Itemtemplate>									

<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Convertion">
								<Itemtemplate>
								    <asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyConvert" Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "QtyConvert"),"#,##0.00") %>' Width="40" />
								    <asp:Label Runat="server" ID="lbUnitC" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'/>
								    <asp:Label Runat="server" ID="Label1" text="/"/>
								    <asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" 
                                        ID="QtyAreal" Maxlength = "9" 
                                        text='<%# DataBinder.Eval(Container.DataItem, "QtyAreal") %>' Width="20px" />
								    <asp:Label Runat="server" ID="lbUnitA" text='<%# DataBinder.Eval(Container.DataItem, "UnitAreal") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Volume (%)" ItemStyle-HorizontalAlign = "Right" SortExpression="Volume">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Volume" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "Volume") %>' Width="40" />
								</Itemtemplate>		

<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rotasi" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:DropDownList ID="Rotasi" CssClass="DropDownList" Width="100%" runat="server"
								    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Rotasi") %>' 
									   DataSourceID="dsGetSection" DataTextField="RotationName" 
                                        DataValueField="RotationCode">                               
									</asp:DropDownList>		
                                </Itemtemplate>									

<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Used Material" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:DropDownList runat="server" CssClass="DropDownList" ID="UsedMaterial"
                                  Width="80px" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "UsedMaterial") %>'  >
                                        <asp:ListItem Value = "Y">Y</asp:ListItem>
                                        <asp:ListItem Value = "N">N</asp:ListItem>
                                        <asp:ListItem Value = ""> </asp:ListItem>
                                        </asp:DropDownList>		
                                </Itemtemplate>									

                                <ControlStyle Width="40px" />

                                <HeaderStyle Width="40px" />

<ItemStyle HorizontalAlign="Right"></ItemStyle>
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
        <asp:SqlDataSource ID="dsGetSection" runat="server" SelectCommand="SELECT RotationCode, RotationName FROM V_MsRotasiFill" ></asp:SqlDataSource>         
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
