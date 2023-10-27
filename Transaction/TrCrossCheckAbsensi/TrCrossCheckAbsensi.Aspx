<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCrossCheckAbsensi.Aspx.vb" Inherits="Transaction_TrCrossCheckAbsensi" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Priority</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script type="text/javascript">
        function OpenPopup() {
            window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
            return false;
        }
        function openprintdlg() {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);
        }

        function OpenPopup() {
            window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
            return false;
        }    
    </script>
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
                  <asp:ListItem Selected="true" Text="EmpNumb" Value="Reference"></asp:ListItem>
                  <asp:ListItem Text="Emp Name" Value="EmpName"></asp:ListItem>
                  <asp:ListItem Text="Division" Value="Division"></asp:ListItem>                            
                  <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>                                           
                  <asp:ListItem Text="JobTiltle" Value="JobTiltle"></asp:ListItem>                            
                                                                                           
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
                  <asp:ListItem Selected="true" Text="EmpNumb" Value="Reference"></asp:ListItem>
                  <asp:ListItem Text="Emp Name" Value="EmpName"></asp:ListItem>
                  <asp:ListItem Text="Division" Value="Division"></asp:ListItem>                            
                  <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>                                           
                  <asp:ListItem Text="JobTiltle" Value="JobTiltle"></asp:ListItem>                                             
                  
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Division</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlDivisi" CssClass="DropDownList" 
                    AutoPostBack="True" Height="19px" Width="239px"> </asp:DropDownList> </td>
             <%--<td> Week Priority</td>
             <td> :</td>
             <td> 
                 <asp:TextBox runat="server" ID ="tbWeek" 
                    CssClass="TextBox"/> 
                <asp:Button ID="btnweek" runat="server" class="btngo" Text="..."/>
               </td>--%>
             <td>
            
            
            &nbsp;&nbsp;&nbsp;&nbsp;									
       <asp:Button ID="btnConfirm" runat="server" class="bitbtndt btnsave" 
            Text="Confirm" validationgroup="Input" Height="18px" Width="82px"/>									
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;									
       <asp:Button ID="btnUnConfirm" runat="server" class="bitbtndt btncancel" Text="UnConfirm" 
            validationgroup="Input" Height="18px" Width="83px"/>									
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            
            </td>
        </tr>
        
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False">
                <TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                &nbsp; s/d&nbsp;&nbsp;&nbsp;&nbsp;
            <BDP:BasicDatePicker ID="tbDate2" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False">
                <TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                &nbsp; </td>
             <td> &nbsp;</td>
             <td> &nbsp;</td>
             <td> 
                 &nbsp;</td>
             <td>
            
            
                 &nbsp;</td>
        </tr>
       
    </table> 
    <br />
     <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Non Confirm" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Confirm" Value="1"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:MultiView ID="MV1" runat="server" ActiveViewIndex="0">
        
         <asp:View ID="Tab1" runat="server">
            <asp:Panel ID="PnlTop" runat="server" Visible="false">
     <%--<asp:Button class="bitbtn btnUnapply" runat="server" ID="BtnRemove" Text="Remove" width="85"/>--%>
    
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
				            
				            <asp:TemplateField HeaderText="Employee Name" >
								<Itemtemplate>
									<asp:Label Width="150px" Runat="server" ID="Employee" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            
				            <asp:TemplateField HeaderText="Employee Name" >
								<Itemtemplate>
									<asp:Label Width="150px" Runat="server" ID="EmployeeName" text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Confirm" >
								<Itemtemplate>
									<asp:Label Width="50px" Runat="server" ID="Confirm" text='<%# DataBinder.Eval(Container.DataItem, "FgConfirm") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							 <asp:TemplateField HeaderText="Division Name" >
								<Itemtemplate>
									<asp:Label Width="150px" Runat="server" ID="DivisionName" text='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
	
							<asp:TemplateField HeaderText="Job Title" >
								<Itemtemplate>
									<asp:Label Width="130px" Runat="server" ID="JobTitle" text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
				      <asp:TemplateField HeaderText="Check" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="40px" CssClass="DropDownList" ID="ddlCheck">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
				      
				      <asp:TemplateField HeaderText="LKM 1" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM1">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision1" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision1" text='<%# DataBinder.Eval(Container.DataItem, "Abs01") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
							              
					  <asp:TemplateField HeaderText="LKM2" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM2">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision2" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision2" text='<%# DataBinder.Eval(Container.DataItem, "Abs02") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM3" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM3">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision3" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision3" text='<%# DataBinder.Eval(Container.DataItem, "Abs03") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>														
		
					  <asp:TemplateField HeaderText="LKM4" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM4">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision4" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision4" text='<%# DataBinder.Eval(Container.DataItem, "Abs04") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM5" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM5">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision5" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision5" text='<%# DataBinder.Eval(Container.DataItem, "Abs05") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM6" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM6">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision6" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision6" text='<%# DataBinder.Eval(Container.DataItem, "Abs06") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  
					  <asp:TemplateField HeaderText="LKM7" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM7">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision7" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision7" text='<%# DataBinder.Eval(Container.DataItem, "Abs07") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM8" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM8">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision8" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision8" text='<%# DataBinder.Eval(Container.DataItem, "Abs08") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM9" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM9">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision9" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision9" text='<%# DataBinder.Eval(Container.DataItem, "Abs09") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM10" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px " CssClass="DropDownList" ID="ddlLKM10">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision10" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision10" text='<%# DataBinder.Eval(Container.DataItem, "Abs10") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM11" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM11">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision11" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision11" text='<%# DataBinder.Eval(Container.DataItem, "Abs11") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM12" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM12">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision12" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision12" text='<%# DataBinder.Eval(Container.DataItem, "Abs12") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM13" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM13">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision13" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision13" text='<%# DataBinder.Eval(Container.DataItem, "Abs13") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM 14" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM14">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision14" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision14" text='<%# DataBinder.Eval(Container.DataItem, "Abs14") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>         
					
    					</Columns>
        </asp:GridView>                
     
       </asp:Panel>  
        </asp:View>
        <asp:View ID="Tab2" runat="server">
       <asp:Panel ID="PnlBottom" runat="server" Visible="false">
       <asp:Button class="bitbtn btnapply" runat="server" ID="btnAdd" Text="Top Priority" 
               width="82px"/>
    
    <asp:GridView id="DataGrid1" runat="server" 
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
				      
				      <asp:TemplateField HeaderText="Employee " >
								<Itemtemplate>
									<asp:Label Width="100px" Runat="server" ID="Employee" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				      
				      <asp:TemplateField HeaderText="Employee Name" >
								<Itemtemplate>
									<asp:Label Width="150px" Runat="server" ID="EmployeeName" text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Confirm" >
								<Itemtemplate>
									<asp:Label Width="50px" Runat="server" ID="Confirm" text='<%# DataBinder.Eval(Container.DataItem, "FgConfirm") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Title" >
								<Itemtemplate>
									<asp:Label Width="130px" Runat="server" ID="JobTitle" text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
				      <asp:TemplateField HeaderText="Check" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="40px" CssClass="DropDownList" ID="ddlCheck">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
				      
				      <asp:TemplateField HeaderText="Decision1" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision1" text='<%# DataBinder.Eval(Container.DataItem, "Abs01") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
							              
					  <asp:TemplateField HeaderText="LKM2" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM2">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision2" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision2" text='<%# DataBinder.Eval(Container.DataItem, "Abs02") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM3" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM3">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision3" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision3" text='<%# DataBinder.Eval(Container.DataItem, "Abs03") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>														
		
					  <asp:TemplateField HeaderText="LKM4" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM4">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision4" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision4" text='<%# DataBinder.Eval(Container.DataItem, "Abs04") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM5" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM5">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision5" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision5" text='<%# DataBinder.Eval(Container.DataItem, "Abs05") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM6" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM6" >
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision6" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision6" text='<%# DataBinder.Eval(Container.DataItem, "Abs06") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  
					  <asp:TemplateField HeaderText="LKM7" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM7">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision7" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision7" text='<%# DataBinder.Eval(Container.DataItem, "Abs07") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM8" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM8">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision8" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision8" text='<%# DataBinder.Eval(Container.DataItem, "Abs08") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM9" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM9">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision9" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision9" text='<%# DataBinder.Eval(Container.DataItem, "Abs09") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM10" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px " CssClass="DropDownList" ID="ddlLKM10">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision10" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision10" text='<%# DataBinder.Eval(Container.DataItem, "Abs10") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM11" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM11">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision11" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision11" text='<%# DataBinder.Eval(Container.DataItem, "Abs11") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM12" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM12">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision12" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision12" text='<%# DataBinder.Eval(Container.DataItem, "Abs12") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="LKM13" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM13">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision13" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision13" text='<%# DataBinder.Eval(Container.DataItem, "Abs13") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
					  <asp:TemplateField HeaderText="LKM 14" >
								<Itemtemplate>
				            <asp:DropDownList runat="server" Width="35px" CssClass="DropDownList" ID="ddlLKM14">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                              </Itemtemplate>														
					  </asp:TemplateField>
					  
					  <asp:TemplateField HeaderText="Decision14" >
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="Decision14" text='<%# DataBinder.Eval(Container.DataItem, "Abs14") %>'/>
								</Itemtemplate>														
					  </asp:TemplateField> 
					  
	
												
    					</Columns>
        </asp:GridView>                
     
       </asp:Panel>
       </asp:View>
        </asp:MultiView>
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
