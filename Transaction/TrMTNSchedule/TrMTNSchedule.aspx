<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMTNSchedule.aspx.vb" Inherits="TrMTNSchedule_TrMTNSchedule" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Schedule</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    
    <%--<script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
--%>
    <script src="../../JQuery/gridviewScroll.min.js" type="text/javascript"></script>
        <script type="text/javascript">   
        $(document).ready(function () { 
            gridviewScroll(); 
        }); 
     
        function gridviewScroll() { 
            $('#<%=DataGrid.ClientID%>').gridviewScroll({ 
                width: '100%', 
                height: '100%', 
                freezesize: 9 
            }); 
        } 
//        function actual()
//        {
//            try
//            {
//                var result = prompt("Remark Close", "");
//                if (result){
//                    document.getElementById("HiddenRemarkClose").value = result;
//                } else {
//                    document.getElementById("HiddenRemarkClose").value = "False Value";
//                }
//                postback();
//                //document.form1.submit();                
//            }catch(err){
//                alert(err.description);
//            }        
//        }
//        
//        function PressFlag()
//        {
//            var _result = false;

//            if(event.keyCode == 89 || event.keyCode==78 || event.keyCode == 110 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39)
//            {
//                _result = true;
//            }
//            else
//            {
//                _result = false;
//            }
//            
//            return _result;
//        }

//        function PressWeek()
//        {
//            var _result = false;

//            if(event.keyCode == 110 || event.keyCode==8 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39 || event.keyCode==188 || ( event.keyCode >= 49 && event.keyCode <= 53 ))
//            {
//                _result = true;
//            }
//            else
//            {
//                _result = false;
//            }
//            
//            return _result;
//        }  
//        
//        function PressMonth()
//        {
//            var _result = false;

//            if(event.keyCode == 110 || event.keyCode==8 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39 || event.keyCode==188 || ( event.keyCode >= 48 && event.keyCode <= 57 ))
//            {
//                _result = true;
//            }
//            else
//            {
//                _result = false;
//            }
//            
//            return _result;
//        }    
//        
//        function postback()
//        {
//            __doPostBack('','');
//        }
//    
    </script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    </head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      
      </asp:ScriptManager>
      
      <div class="Content">
     <div class="H1">Maintenance Schedule</div>
     <hr style="color:Blue" />
     <table>
     <tr>
     <td valign="top">
         <table>
            <tr>
                <td style="width:100px;text-align:right">Quick Search :</td>            
                <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                      <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                          <asp:ListItem Selected="True" Value="A.MTN_Type_Name">MTN Type</asp:ListItem>  
                          <asp:ListItem Value="A.MTN_Item">MTN Item</asp:ListItem>
                          <asp:ListItem Value="A.MTN_Item_Name">MTN Item Name</asp:ListItem>                                            
                          <asp:ListItem Value="B.Job">Job</asp:ListItem>
                          <asp:ListItem Value="B.JobName">Job Name</asp:ListItem>
                          <asp:ListItem Value="B.MTNPattern">Pattern</asp:ListItem>
                      </asp:DropDownList>  
                      <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" Width="50px" AutoPostBack ="true" />                
                      <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                </td>
             </tr>   
             <tr>
                <td colspan="2">
                     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                      <table>
                        <tr>
                          <td style="width:100px;text-align:right">
                              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
                              </asp:DropDownList>
                          </td>
                          <td>
                           
                              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
                              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >
                                      <asp:ListItem Selected="True" Value="A.MTN_Type_Name">MTN Type</asp:ListItem>  
                                      <asp:ListItem Value="A.MTN_Item">MTN Item</asp:ListItem>
                                      <asp:ListItem Value="A.MTN_Item_Name">MTN Item Name</asp:ListItem>                                            
                                      <asp:ListItem Value="B.Job">Job</asp:ListItem>
                                      <asp:ListItem Value="B.JobName">Job Name</asp:ListItem>
                                      <asp:ListItem Value="B.MTNPattern">Pattern</asp:ListItem>
                              </asp:DropDownList>
                          </td>              
                        </tr>        
                      </table>      
                      </asp:Panel>
                </td>
              </tr>   
              <tr>
            <td style="width:100px;text-align:right">MTN Section : </td>         
            <td>
                <asp:TextBox ID="tbSection" runat="server" CssClass="TextBox" Width="60px" 
                    AutoPostBack="true" />
                <asp:TextBox ID="tbSectionName" runat="server" CssClass="TextBoxR" 
                    Width="220px" Enabled="False" />
                <asp:Button Class="btngo" ID="btnSection" Text="..." runat="server" 
                    ValidationGroup="Input" />                 
            </td>
            </tr>            
          </table>
          <table>
          <tr>
                <td>
                <fieldset style="width:430px">
                    <legend>Copy To</legend>
                    <table>
                    <tr>
                        <td>MTN Section : </td>
                        <td><asp:Label ID="lbCode" runat="server" CssClass="Label" Width="50px" Enabled="false" />
                        <asp:Label ID="lbName" runat="server" CssClass="Label" Width="50px" Enabled="false" /></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>From : </td>                                       
                        <td><asp:DropDownList ID="ddlYearFrom" runat="server" CssClass="DropDownList" Width="50px" />
                         To 
                        <asp:DropDownList ID="ddlYearTo" runat="server" CssClass="DropDownList" Width="50px" /> </td>
                        <%--<td><asp:DropDownList runat="server" ID="ddlSchedule" CssClass="DropDownList" >
                         <asp:ListItem Selected="true" Text="Y" Value="Y"></asp:ListItem>
                         <asp:ListItem Text="N" Value="N"></asp:ListItem>
                      </asp:DropDownList></td>--%>
                        <td><asp:Button class="bitbtn btngetitem" runat="server" ID="btnGo" 
                        Text="GO" ValidationGroup="Input"/></td>
                    </tr>
                    
                    </table>
                                
                      
                   </fieldset>
                </td>
            </tr>
          </table>
     </td>
     <td valign="top">
         <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbReset" Text="Click button to reset with Default Formula : "></asp:Label>
                        <asp:Button class="bitbtn btngetitem" runat="server" ID="btnReset" AutoPostBack="True"
                        Text="Reset Data" width="90" />
                    </td>
                </tr>                   
                <tr>
                    <td>               
                   <fieldset style="width:430px">
                    <legend>Generate Schedule</legend>
                    <table>
                    <tr>
                        <td>Pattern</td>
                        <td>Pattern Name</td>
                        <%--<td>Schedule</td>--%>
                        <td></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbPattern" runat="server" CssClass="TextBoxR" Width="50px" Enabled="false" /></td>
                        <td><asp:DropDownList ID="ddlPattern" runat="server" CssClass="DropDownList" Width="180px" AutoPostBack="true"/></td>
                        <%--<td><asp:DropDownList runat="server" ID="ddlSchedule" CssClass="DropDownList" >
                         <asp:ListItem Selected="true" Text="Y" Value="Y"></asp:ListItem>
                         <asp:ListItem Text="N" Value="N"></asp:ListItem>
                      </asp:DropDownList></td>--%>
                        <td><asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" 
                        Text="Generate" ValidationGroup="Input"/></td>
                    </tr>
                    
                    </table>
                                
                      
                   </fieldset>
                </td>
                </tr>                        
            </table>     
     </td>
     </tr>
     </table>
     
     <br />       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>    
      <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
      <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" Width="1500px" PageSize="20" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="False"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />					  
				<EmptyDataTemplate>				    
				</EmptyDataTemplate>	  
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
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="20" >
								<ItemTemplate >
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" Width="50" />
								    <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" Width="55" OnClientClick="return confirm('Sure to clear this data?');" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" Width="50" CommandName="Update" />																						 																		
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" Width="55" CommandName="Cancel" />																						 																		
								</EditItemTemplate>
								<HeaderStyle Width="20px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="MTN Type" HeaderStyle-Width="100" SortExpression="MTNType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MTNType" text='<%# DataBinder.Eval(Container.DataItem, "MTNType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MTNTypeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MTNType") %>'>
									</asp:Label>
								</EditItemTemplate>
								<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="MTN Item" HeaderStyle-Width="100" SortExpression="MTNItem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MTNItem" text='<%# DataBinder.Eval(Container.DataItem, "MTNItem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MTNItemEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MTNItem") %>'>
									</asp:Label>
								</EditItemTemplate>
								<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="MTN Item Name" HeaderStyle-Width="100" SortExpression="MTNItemName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MTNItemName" text='<%# DataBinder.Eval(Container.DataItem, "MTNItemName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="MTNItemNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MTNItemName") %>'>
									</asp:Label>
																
								</EditItemTemplate>
								<HeaderStyle Width="100px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job" HeaderStyle-Width="50" SortExpression="Job">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="JobEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>							
								</EditItemTemplate>
                                <HeaderStyle Width="50px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name" ItemStyle-HorizontalAlign = "Left"  HeaderStyle-Width="150" SortExpression="JobName">
								<Itemtemplate >
									<asp:Label Runat="server" MaxLength="100" Width="100%" ID="JobName"  text='<%#DataBinder.Eval(Container.DataItem,"JobName")%>'> 
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="JobNamedit" MaxLength="100" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'>
									</asp:Label>
								</EditItemTemplate>
							    <HeaderStyle Width="200px" />
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Last Schedule" ItemStyle-HorizontalAlign = "Left"  HeaderStyle-Width="30" SortExpression="LastSchedule">
								<Itemtemplate>
									<asp:Label Runat="server" Width="50" ID="LastSchedule" text='<%#DataBinder.Eval(Container.DataItem,"LastSchedule")%>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="LastScheduledit"  CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "LastSchedule") %>'>
									</asp:Label>
								</EditItemTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Pattern" ItemStyle-HorizontalAlign = "Left"  HeaderStyle-Width="30" SortExpression="Pattern">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Interval" text='<%# DataBinder.Eval(Container.DataItem, "Pattern") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="IntervalEdit" MaxLength="4" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "Pattern") %>'>
									</asp:Label>
								</EditItemTemplate>
								
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="30" SortExpression="Status">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Status" text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="StatusEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
									</asp:Label>							
								</EditItemTemplate>
                                <HeaderStyle Width="50px"></HeaderStyle>
							</asp:TemplateField>
							
											<asp:TemplateField HeaderText="Jan 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0101">
								<%--<HeaderTemplate>
                                    Jan 1
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Januari1" text='<%# DataBinder.Eval(Container.DataItem, "0101") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Januari1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0101") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jan 2" HeaderStyle-Width="100" ItemStyle-HorizontalAlign = "Right" SortExpression="0102">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Januari2" text='<%# DataBinder.Eval(Container.DataItem, "0102") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Januari2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0102") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jan 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0103">
								<%--<HeaderTemplate>
                                    Jan 3
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Januari3" text='<%# DataBinder.Eval(Container.DataItem, "0103") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Januari3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0103") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jan 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0104">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Januari4" text='<%# DataBinder.Eval(Container.DataItem, "0104") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Januari4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0104") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jan 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0105">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Januari5" text='<%# DataBinder.Eval(Container.DataItem, "0105") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Januari5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0105") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Feb 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0201">
								<%--<HeaderTemplate>
                                    Feb 1
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Febuari1" text='<%# DataBinder.Eval(Container.DataItem, "0201") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Febuari1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0201") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Feb 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0202">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Febuari2" text='<%# DataBinder.Eval(Container.DataItem, "0202") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Febuari2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0202") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Feb 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0203">
								<%--<HeaderTemplate>
                                    Febuari
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Febuari3" text='<%# DataBinder.Eval(Container.DataItem, "0203") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Febuari3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0203") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Feb 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0204">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Febuari4" text='<%# DataBinder.Eval(Container.DataItem, "0204") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Febuari4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0204") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Feb 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0205">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Febuari5" text='<%# DataBinder.Eval(Container.DataItem, "0205") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Febuari5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0205") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mar 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0301">
								<%--<HeaderTemplate>
                                    Maret
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Maret1" text='<%# DataBinder.Eval(Container.DataItem, "0301") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Maret1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0301") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mar 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0302">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Maret2" text='<%# DataBinder.Eval(Container.DataItem, "0302") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Maret2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0302") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mar 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0303">
							<%--	<HeaderTemplate>
                                    Maret
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Maret3" text='<%# DataBinder.Eval(Container.DataItem, "0303") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Maret3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0303") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mar 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0304">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Maret4" text='<%# DataBinder.Eval(Container.DataItem, "0304") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Maret4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0304") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mar 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0305">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Maret5" text='<%# DataBinder.Eval(Container.DataItem, "0305") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Maret5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0305") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Apr 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0401">
								<%--<HeaderTemplate>
                                    April
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="April1" text='<%# DataBinder.Eval(Container.DataItem, "0401") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="April1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0401") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Apr 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0402">
								<Itemtemplate>
									<asp:Label Runat="server" ID="April2" text='<%# DataBinder.Eval(Container.DataItem, "0402") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="April2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0402") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Apr 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0403">
								<%--<HeaderTemplate>
                                    April
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="April3" text='<%# DataBinder.Eval(Container.DataItem, "0403") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="April3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0403") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Apr 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0404">
								<Itemtemplate>
									<asp:Label Runat="server" ID="April4" text='<%# DataBinder.Eval(Container.DataItem, "0404") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="April4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0404") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Apr 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0405">
								<Itemtemplate>
									<asp:Label Runat="server" ID="April5" text='<%# DataBinder.Eval(Container.DataItem, "0405") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="April5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0405") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mei 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0501">
								<%--<HeaderTemplate>
                                    Mei
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Mei1" text='<%# DataBinder.Eval(Container.DataItem, "0501") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Mei1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0501") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mei 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0502">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Mei2" text='<%# DataBinder.Eval(Container.DataItem, "0502") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Mei2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0502") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mei 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0503">
								<%--<HeaderTemplate>
                                    Mei
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Mei3" text='<%# DataBinder.Eval(Container.DataItem, "0503") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Mei3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0503") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mei 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0504">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Mei4" text='<%# DataBinder.Eval(Container.DataItem, "0504") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Mei4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0504") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Mei 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0505">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Mei5" text='<%# DataBinder.Eval(Container.DataItem, "0505") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Mei5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0505") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jun 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0601">
								<%--<HeaderTemplate>
                                    Juni
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juni1" text='<%# DataBinder.Eval(Container.DataItem, "0601") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juni1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0601") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jun 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0602">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juni2" text='<%# DataBinder.Eval(Container.DataItem, "0602") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juni2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0602") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jun 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0603">
								<%--<HeaderTemplate>
                                    Juni
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juni3" text='<%# DataBinder.Eval(Container.DataItem, "0603") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juni3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0603") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jun 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0604">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juni4" text='<%# DataBinder.Eval(Container.DataItem, "0604") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juni4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0604") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jun 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0605">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juni5" text='<%# DataBinder.Eval(Container.DataItem, "0605") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juni5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0605") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jul 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0701">
								<%--<HeaderTemplate>
                                    Juli
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juli1" text='<%# DataBinder.Eval(Container.DataItem, "0701") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juli1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0701") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jul 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0702">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juli2" text='<%# DataBinder.Eval(Container.DataItem, "0702") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juli2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0702") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jul 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0703">
								<%--<HeaderTemplate>
                                    Juli
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juli3" text='<%# DataBinder.Eval(Container.DataItem, "0703") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juli3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0703") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jul 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0704">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juli4" text='<%# DataBinder.Eval(Container.DataItem, "0704") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juli4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0704") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Jul 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0705">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Juli5" text='<%# DataBinder.Eval(Container.DataItem, "0705") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Juli5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0705") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Aug 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0801">
								<%--<HeaderTemplate>
                                    Agustus
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Agustus1" text='<%# DataBinder.Eval(Container.DataItem, "0801") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Agustus1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0801") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Aug 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0802">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Agustus2" text='<%# DataBinder.Eval(Container.DataItem, "0802") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Agustus2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0802") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Aug 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0803">
								<%--<HeaderTemplate>
                                    Agustus
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Agustus3" text='<%# DataBinder.Eval(Container.DataItem, "0803") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Agustus3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0803") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Aug 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0804">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Agustus4" text='<%# DataBinder.Eval(Container.DataItem, "0804") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Agustus4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0804") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Aug 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0805">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Agustus5" text='<%# DataBinder.Eval(Container.DataItem, "0805") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Agustus5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0805") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Sep 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0901">
								<%--<HeaderTemplate>
                                    September
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="September1" text='<%# DataBinder.Eval(Container.DataItem, "0901") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="September1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0901") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Sep 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0902">
								<Itemtemplate>
									<asp:Label Runat="server" ID="September2" text='<%# DataBinder.Eval(Container.DataItem, "0902") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="September2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0902") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Sep 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0903">
								<%--<HeaderTemplate>
                                    September
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="September3" text='<%# DataBinder.Eval(Container.DataItem, "0903") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="September3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0903") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Sep 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0904">
								<Itemtemplate>
									<asp:Label Runat="server" ID="September4" text='<%# DataBinder.Eval(Container.DataItem, "0904") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="September4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0904") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Sep 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="0905">
								<Itemtemplate>
									<asp:Label Runat="server" ID="September5" text='<%# DataBinder.Eval(Container.DataItem, "0905") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="September5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "0905") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Okt 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1001">
								<%--<HeaderTemplate>
                                    Oktober
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Oktober1" text='<%# DataBinder.Eval(Container.DataItem, "1001") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Oktober1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1001") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Okt 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1002">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Oktober2" text='<%# DataBinder.Eval(Container.DataItem, "1002") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Oktober2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1002") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Okt 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1003">
								<%--<HeaderTemplate>
                                    Oktober
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Oktober3" text='<%# DataBinder.Eval(Container.DataItem, "1003") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Oktober3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1003") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Okt 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1004">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Oktober4" text='<%# DataBinder.Eval(Container.DataItem, "1004") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Oktober4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1004") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Okt 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1005">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Oktober5" text='<%# DataBinder.Eval(Container.DataItem, "1005") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Oktober5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1005") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Nov 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1101">
								<%--<HeaderTemplate>
                                    November
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="November1" text='<%# DataBinder.Eval(Container.DataItem, "1101") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="November1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1101") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Nov 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1102">
								<Itemtemplate>
									<asp:Label Runat="server" ID="November2" text='<%# DataBinder.Eval(Container.DataItem, "1102") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="November2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1102") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Nov 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1103">
							<%--	<HeaderTemplate>
                                    November
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="November3" text='<%# DataBinder.Eval(Container.DataItem, "1103") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="November3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1103") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Nov 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1104">
								<Itemtemplate>
									<asp:Label Runat="server" ID="November4" text='<%# DataBinder.Eval(Container.DataItem, "1104") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="November4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1104") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Nov 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1105">
								<Itemtemplate>
									<asp:Label Runat="server" ID="November5" text='<%# DataBinder.Eval(Container.DataItem, "1105") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="November5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1105") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Des 1" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1201">
								<%--<HeaderTemplate>
                                    Desember
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Desember1" text='<%# DataBinder.Eval(Container.DataItem, "1201") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Desember1Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1201") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Des 2" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1202">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Desember2" text='<%# DataBinder.Eval(Container.DataItem, "1202") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Desember2Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1202") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Des 3" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1203">
								<%--<HeaderTemplate>
                                    Desember
                                </HeaderTemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" ID="Desember3" text='<%# DataBinder.Eval(Container.DataItem, "1203") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Desember3Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1203") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Des 4" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1204">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Desember4" text='<%# DataBinder.Eval(Container.DataItem, "1204") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Desember4Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1204") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Des 5" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="1205">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Desember5" text='<%# DataBinder.Eval(Container.DataItem, "1205") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Textbox Runat="server" ID="Desember5Edit" MaxLength="1" CssClass="TextBox" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "1205") %>'>
									</asp:Textbox>							
								</EditItemTemplate>
								<HeaderStyle Width="50px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							
							<%--------------------------------------------------------------------------------------------------------%>
    					</Columns>
        </asp:GridView>
        </div>
        </div>
    </form>
</body>

</html>
