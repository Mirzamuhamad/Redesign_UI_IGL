<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProcessPayrollAbsence.aspx.vb" Inherits="MsProcessPayrollAbsence_MsProcessPayrollAbsence" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Process Payroll Absence</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 94px;
        }
    </style>
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
      
      <div class="Content">
     <div class="H1">Process Payroll Absence</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Process Code" Value="ProcessCode"></asp:ListItem>
                  <asp:ListItem Text="Employee Numb" Value="EmpNumb"></asp:ListItem>
                  <asp:ListItem Text="Employee Name" Value="Emp_Name"></asp:ListItem>
                  <asp:ListItem Text="Department Name" Value="Department_Name"></asp:ListItem>
                  <asp:ListItem Text="Job Title Name" Value="Job_Title_Name"></asp:ListItem>
                  <asp:ListItem Text="Job Level Name" Value="Job_Level_Name"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <%--<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>--%>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />                           
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
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="Process Code" Value="ProcessCode"></asp:ListItem>
                     <asp:ListItem Text="Employee Numb" Value="EmpNumb"></asp:ListItem>
                     <asp:ListItem Text="Employee Name" Value="Emp_Name"></asp:ListItem> 
                     <asp:ListItem Text="Department Name" Value="Department_Name"></asp:ListItem>
                     <asp:ListItem Text="Job Title Name" Value="Job_Title_Name"></asp:ListItem>
                     <asp:ListItem Text="Job Level Name" Value="Job_Level_Name"></asp:ListItem>       
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
     <br />
     <table>   
        <tr>
            <td class="style1" style="text-align: right">
                Process Code</td>
            <td>
                :</td>
            <td colspan="4">
                <asp:TextBox ID="tbProcessCode" runat="server" CssClass="TextBox" Width="106px" 
                    AutoPostBack="true" />
                <asp:Button Class="btngo" ID="btnProcessCode" Text="..." runat="server" ValidationGroup="Input" />                 
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Data" ValidationGroup="Input"/>
            </td>
        </tr>
     </table>     
      <br />
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>    
      <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
      <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" Width="1500px"
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>			      
							
							<%--<asp:TemplateField HeaderText="Emp Numb" HeaderStyle-Width="300" SortExpression="EmpNumb">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpNumb" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="EmpNumbEdit" Text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>							
								</EditItemTemplate>
							</asp:TemplateField>--%>
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnSetDefault" Text="Set Default" CommandName="SetDefault" width='85' CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>																						 											
									<%--<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />--%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
								</EditItemTemplate>
								<%--<FooterTemplate>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
								</FooterTemplate>--%><HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Process Code" HeaderStyle-Width="100" SortExpression="ProcessCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProcessCode" text='<%# DataBinder.Eval(Container.DataItem, "ProcessCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProcessCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProcessCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="ProcessCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ProcessCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProcessCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%><HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Emp Numb" HeaderStyle-Width="100" SortExpression="EmpNumb">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpNumb" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="EmpNumbEdit" Text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>
									<%--<asp:TextBox Runat="server" ID="FrequencyNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "FrequencyName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FrequencyNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FrequencyNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>	--%>								
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="FrequencyNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="FrequencyNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FrequencyNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%><HeaderStyle Width="100px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Emp Name" HeaderStyle-Width="300" SortExpression="Emp_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Emp_Name" text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="Emp_NameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>							
								</EditItemTemplate>
                                <HeaderStyle Width="300px"></HeaderStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<%--<asp:TemplateField HeaderText="Day Work System" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="30" SortExpression="DayWorkSystem">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="DayWorkSystem" text='<%#DataBinder.Eval(Container.DataItem,"DayWorkSystem")%>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DayWorkSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "DayWorkSystem") %>'>
									</asp:Label>
								</EditItemTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>--%>
							<asp:TemplateField HeaderText="Day Work" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="30" SortExpression="DayWork">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Daywork" text='<%# DataBinder.Eval(Container.DataItem, "DayWork") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DayWorkEdit" MaxLength="4" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "DayWork") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="XTimeAdd" CssClass="TextBox" Width="100%" MaxLength="9" 
                                        Runat="Server">1</asp:TextBox>
								    <cc1:TextBoxWatermarkExtender ID="XTimeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="XTimeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Hadir System" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="30" SortExpression="QtyHadirSystem">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="DayHadirSystem" TEXT='<%# DataBinder.Eval(Container.DataItem, "QtyHadirSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DayHadirSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "QtyHadirSystem") %>'>
									</asp:Label>
								</EditItemTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Hadir Process" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="30" SortExpression="QtyHadir">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="QtyHadir" TEXT='<%# DataBinder.Eval(Container.DataItem, "QtyHadir") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="QtyHadirEdit" MaxLength="4" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "QtyHadir") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="InMonthAdd" CssClass="TextBox" Width="100%" MaxLength="9" 
                                        Runat="Server">1</asp:TextBox>
								    <cc1:TextBoxWatermarkExtender ID="InMonthAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="InMonthAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Alpha System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyAlphaSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DayAlphaSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtyAlphaSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DayAlphaSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyAlphaSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Alpha Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyAlpha">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyAlpha" text='<%# DataBinder.Eval(Container.DataItem, "QtyAlpha") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtyAlphaEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyAlpha") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Sakit System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtySakitSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DaySakitSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtySakitSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DaySakitSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtySakitSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Sakit Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtySakit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtySakit" text='<%# DataBinder.Eval(Container.DataItem, "QtySakit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtySakitEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtySakit") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Izin System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyIzinSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DayIzinSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DayIzinSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Izin Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyIzin">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyIjin" text='<%# DataBinder.Eval(Container.DataItem, "QtyIzin") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtyIjinEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyIzin") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Izin Alpha System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyIzinAlphaSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DayIzinAplhaSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinAlphaSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DayIzinAplhaSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinAlphaSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Izin Alpha Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyIzinAlpha">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyIjinAlpha" text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinAlpha") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtyIjinAlphaEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyIzinAlpha") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Dinas System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyDinasSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DayDinasSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtyDinasSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DayDinasSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyDinasSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Dinas Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyDinas">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyDinas" text='<%# DataBinder.Eval(Container.DataItem, "QtyDinas") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtyDinasEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyDinas") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Day Cuti System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyCutiSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DayCutiSystem" text='<%# DataBinder.Eval(Container.DataItem, "QtyCutiSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="DayCutiSystemEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyCutiSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Day Cuti Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyCuti">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyCuti" text='<%# DataBinder.Eval(Container.DataItem, "QtyCuti") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="QtyCutiEdit" MaxLength="4" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyCuti") %>'>
									</asp:TextBox>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>	
							<%--------------------------------------------------------------------------------------------------------%>
							<asp:TemplateField HeaderText="Total Lembur System" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="TotalLemburSystem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TotalLemburSystem" text='<%# DataBinder.Eval(Container.DataItem, "TotalLemburSystem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="TotalLemburSystemEdit" MaxLength="9" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "TotalLemburSystem") %>'>
									</asp:Label>							
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Total Lembur Process" HeaderStyle-Width="30" ItemStyle-HorizontalAlign = "Right" SortExpression="TotalLembur">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TotalLembur" text='<%# DataBinder.Eval(Container.DataItem, "TotalLembur") %>'>
									</asp:Label>
									<%--text='<%# DataBinder.Eval(Container.DataItem, "TotalLembur") %>'--%>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="TotalLemburEdit" MaxLength="9" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "TotalLembur") %>'>
									</asp:TextBox>	
									<%--text='<%# String.Format("{0:###,###.####}",Convert.ToInt64(DataBinder.Eval(Container.DataItem, "CurrRate"))) %>'--%>						
									<%--Text='<%# DataBinder.Eval(Container.DataItem, "TotalLembur") %>'--%>
								</EditItemTemplate>
								<HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							<%--------------------------------------------------------------------------------------------------------%>
    					</Columns>
        </asp:GridView>
        </div>
        </div>
    </form>
</body>

</html>
