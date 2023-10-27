<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPayrollEmp.aspx.vb" Inherits="Assign_MsPayrollEmp_MsPayrollEmp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 

</script>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>    
    <form id="form1" runat="server">
    <div class="Content">   
    <div class="H1">Payroll Slip Employee</div>
    <hr style="color:Blue" /> 
        <br />                     
        
              <table width="100%">
                <tr>
                    <td style="width:100px">Payroll</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="120px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/> 
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />            
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnNewSlip" Text="New Slip" Visible="false"/>                                                         
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdjustSlip" Text="Adjustment Slip" Visible="false" Width="120px"/>                                                         
                    </td>
                    
                </tr>  
		</table> 
		<asp:Panel runat="server" ID="pnlquick" Visible="true">
		<table> 
                <tr>
                    <td style="width:100px">Quick Search</td>  
                     <td>:</td>      
                     <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                        <asp:ListItem Value="A.EmpNumb">Emp No</asp:ListItem>                                        
                            <asp:ListItem Value="Emp_Name">Emp Name</asp:ListItem>                    
                  </asp:DropDownList>
                                   
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearchFilter" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
            </td>
            
        </tr>               
              </table> 
		 </asp:Panel>  
              <table>
        
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr>
<%--        <td style="width:100px"></td> --%>
        
          <td style="width:114px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
          <td>
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >                     
                    <asp:ListItem Value="A.EmpNumb">Emp No</asp:ListItem>                                        
                    <asp:ListItem Value="Emp_Name">Emp Name</asp:ListItem>                    
                    
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>  
      <br />      
               <asp:Panel runat="server" ID="PnlAssign">          
               <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGrid" runat="server" ShowFooter="False" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Employee Code" HeaderStyle-Width="70" SortExpression="EmpNumb">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="EmpNumb" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="EmpNumbEdit" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EmpNumbAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="150" SortExpression="Emp_Name">
								<Itemtemplate>
									<asp:Label Runat="server" Width="200" ID="EmpName" text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" Width="200" ID="EmpNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "Emp_Name") %>'>
									</asp:Label>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EmpNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Job Title" HeaderStyle-Width="150" SortExpression="Job_Title_Name">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="JobTitle" text='<%# DataBinder.Eval(Container.DataItem, "Job_Title_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="JobTitleEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Job_Title_Name") %>'>
									</asp:Label>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="JobTitleAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>
							
							<%--<asp:TemplateField HeaderText="Section" HeaderStyle-Width="150" SortExpression="Section_Name">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="Section" text='<%# DataBinder.Eval(Container.DataItem, "Section_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SectionEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Section_Name") %>'>
									</asp:Label>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="SectionAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="Eff. Date" HeaderStyle-Width="150" SortExpression="StartDate">
								<Itemtemplate>									
									<asp:Label Runat="server" Width="130" ID="EffDate" text='<%# DataBinder.Eval(Container.DataItem, "Start_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <BDP:BasicDatePicker ID="EffDateEdit" runat="server" DateFormat="dd MMM yyyy" 
                                        ReadOnly = "false" ValidationGroup="Input"
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                        DisplayType="TextBoxAndImage" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' 
                                        TextBoxStyle-CssClass="TextDate" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
									<%--<asp:Label Runat="server" ID="EffDateEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'>
									</asp:Label>--%>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="EffDateAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Curr" HeaderStyle-Width="40" SortExpression="Currency">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Currency" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CurrencyEdit" Width="100%" 
                                        CssClass="DropDownList" AutoPostBack="True" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'
                                        DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                        DataValueField="Currency">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CurrencyAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                        DataValueField="Currency">                                         
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="210px" />
							</asp:TemplateField>																						
										
							<asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70" SortExpression="Amount" ItemStyle-HorizontalAlign="Right">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="Amount" text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="AmountEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>' Runat="Server"/>								   
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AmountAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>	
										
                            <asp:TemplateField HeaderText="Formula" HeaderStyle-Width="40" SortExpression="FormulaName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Formula" text='<%# DataBinder.Eval(Container.DataItem, "FormulaName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FormulaEdit" Width="100%" 
                                        CssClass="DropDownList" AutoPostBack="True" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'
                                        DataSourceID="dsFormula" DataTextField="FormulaName" 
                                        DataValueField="FormulaCode">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FormulaAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsFormula" DataTextField="FormulaName" 
                                        DataValueField="FormulaCode">                                         
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="210px" />
							</asp:TemplateField>										
							
							<asp:TemplateField HeaderText="Remark" HeaderStyle-Width="100" SortExpression="Remark">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="Remark" text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="RemarkEdit" CssClass="TextBox" Width="100%" MaxLength="255" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RemarkAdd" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>			
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
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
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView> 
        </asp:Panel>
        <asp:Panel ID="pnlNewSlip" runat="server" Visible ="false">
            <table width="100%">
                <tr>
                    <td style="width:100px">Effective Date</td>
                    <td>:</td>
                    <td>                                           
                        <BDP:BasicDatePicker ID="tbEffDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" AutoPostBack="true"
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td style="width:100px">Currency</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlCurr" runat = "server" Width="120px" ></asp:DropDownList>                            
                    </td>
                </tr> 
                <tr>
                    <td style="width:100px">Amount</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbAmount" runat="server" Width="120px" />
                    </td>
                </tr>    
                <tr>
                    <td style="width:100px">Formula</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlFormula" runat = "server" Width="250px" ></asp:DropDownList>                            
                    </td>
                </tr> 
                <tr>
                    <td style="width:100px">Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Id="tbRemark" runat="server" Width="350px" />
                    </td>
                </tr>  
                <tr>
                    <td>
                    
                    </td>
                </tr> 
            </table>
            <fieldset style="width:300px">
                    <legend> Apply for : </legend>
                        <asp:RadioButtonList ID="rbEmp" runat="server" RepeatColumns="2" Width="300px" >
                        <asp:ListItem Value= "0" Selected="True">All Employee</asp:ListItem>
                        <asp:ListItem Value= "1">Selected Employee</asp:ListItem>                        
                    </asp:RadioButtonList>                
                    </fieldset>
                    
                    <table width="100%" visible=False>
                    <br/>
                    Employee Available
                    <tr>
                    
                    <%--<td>
                        <asp:Label ID="lblAssign" runat="server" Text="Employee Available"></asp:Label>
                    </td>--%>
                    <td style="width:40%;">
                        <%--<div style="border:0px  solid; width:100%; height:300px; overflow:auto;">--%>
                        <dx:ASPxGridView ID="AvailableGrid" runat="server" Width="100%" style="table-layout:fixed;"
                        EmptyDataText="There are no data records to display." KeyFieldName = "Emp_No"                     
                        AllowPaging="True" DataSourceID="dsAvailable" AutoGenerateColumns="False">
                        <Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                        <HeaderTemplate><input type="checkbox" onclick="AvailableGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn Caption="Employee Code" FieldName="Emp_No" VisibleIndex="1" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Employee Name" FieldName="Emp_Name" VisibleIndex="2" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>                        
                        <dx:GridViewDataColumn Caption="Job Title" FieldName="Job_Title_Name" VisibleIndex="3" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Department" FieldName="Dept_Name" VisibleIndex="4" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                    </dx:ASPxGridView>
                  <%--</div>  --%>
                    <asp:SqlDataSource ID="dsAvailable" runat="server" />                    
                </td>
            </tr>
            <tr>
                    <td colspan = "3">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnOKSlip" Text="OK" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelSlip" Text="Cancel" />                                                     
                    </td>                                   
                </tr>
          </table>
        </asp:Panel>
        <asp:Panel ID="pnlNewAdjust" runat="server" Visible ="false">
            <table width="100%">
                <tr>
                    <td style="width:100px">Effective Date</td>
                    <td>:</td>
                    <td>                                           
                        <BDP:BasicDatePicker ID="tbEffAdjust" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" AutoPostBack="true"
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td style="width:100px">Amount Adjust</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlAdjust" runat = "server" Width="100px" >
                          <asp:ListItem Selected="True">Percentage</asp:ListItem>
                          <asp:ListItem>Amount</asp:ListItem>                          
                        </asp:DropDownList>     
                        <asp:TextBox CssClass="TextBox" Id="tbAmountAdjust" runat="server" Width="120px" />                       
                    </td>
                </tr> 
                <tr>
                    <td style="width:100px">Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Id="tbRemarkAdjust" runat="server" Width="350px" />
                    </td>
                </tr>  
                <tr>
                    <td>
                    
                    </td>
                </tr> 
            </table>
            <fieldset style="width:300px">
                    <legend> Apply for : </legend>
                        <asp:RadioButtonList ID="rbEmpAdjust" runat="server" RepeatColumns="2" Width="300px" >
                        <asp:ListItem Value= "0" Selected="True">All Employee</asp:ListItem>
                        <asp:ListItem Value= "1">Selected Employee</asp:ListItem>                        
                    </asp:RadioButtonList>                
                    </fieldset>
                    
                    <table width="100%" visible=False>
                    <br/>
                    Employee Available
                    <tr>
                    
                    <%--<td>
                        <asp:Label ID="lblAssign" runat="server" Text="Employee Available"></asp:Label>
                    </td>--%>
                    <td style="width:40%;">
                        <%--<div style="border:0px  solid; width:100%; height:300px; overflow:auto;">--%>
                        <dx:ASPxGridView ID="GridAdjust" runat="server" Width="100%" style="table-layout:fixed;"
                        EmptyDataText="There are no data records to display." KeyFieldName = "Emp_No"                     
                        AllowPaging="True" DataSourceID="dsAdjust" AutoGenerateColumns="False">
                        <Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                        <HeaderTemplate><input type="checkbox" onclick="GridAdjust.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn Caption="Employee Code" FieldName="Emp_No" VisibleIndex="1" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Employee Name" FieldName="Emp_Name" VisibleIndex="2" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>                        
                        <dx:GridViewDataColumn Caption="Job Title" FieldName="Job_Title_Name" VisibleIndex="3" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Effective Date" FieldName="Start_Date" VisibleIndex="4" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Amount" FieldName="Amount" VisibleIndex="5" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Formula" FieldName="Formula" VisibleIndex="6" > <Settings AutoFilterCondition="Contains" /> </dx:GridViewDataColumn>                        
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                    </dx:ASPxGridView>
                  <%--</div>  --%>
                    <asp:SqlDataSource ID="dsAdjust" runat="server" />
                </td>
            </tr>
            <tr>
                    <td colspan = "3">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnOKAdjust" Text="OK" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelAdjust" Text="Cancel" />                                                     
                    </td>                                   
                </tr>
          </table>
        </asp:Panel>
    </div>
    <asp:SqlDataSource ID="dsCurrency" runat="server" 
          SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="dsFormula" runat="server" 
          SelectCommand="EXEC S_GetFormula">
    </asp:SqlDataSource>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>
