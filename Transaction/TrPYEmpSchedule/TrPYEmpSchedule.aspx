<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYEmpSchedule.aspx.vb" Inherits="Transaction_TrPYEmpSchedule_TrPYEmpSchedule" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
           TNstr = TNstr.toFixed(digit);                
        } 
        nStr = TNstr;        
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }   
            
       function setformatdt()
        {
        try
         {         
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');                 
        }catch (err){
            alert(err.description);
          }      
        }     
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {   text-align:right;
        }
        .style6
        {
            width: 90px;
            text-align:right;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Schedule Shift</div>
     <hr style="color:Blue" />        
      
      <table>
     
        <tr>
            <td style="width:100px;text-align:right">Schedule Date :</td>         
            <td><BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="true"
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
            <td> s/d </td>
            <td>
                <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="true"
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                            
            </td>
            <td>
                <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnSchedule" Text="View Schedule" />
                <asp:Button class="bitbtn btnpreview" Width="112px" runat="server" ID="btnUnSchedule" Text="View Un-Schedule" />
            </td>
            </tr>
            <tr>
            <td style="width:100px;text-align:right">Organization :</td>
            <td colspan="3"><asp:DropDownList runat="server" AutoPostBack="true" ID="ddlDepartment" CssClass="DropDownList" Width="200px" /></td>            
            <td><asp:Button class="bitbtn btncopy" Width="98px" runat="server" ID="btnCopy" Text="Copy Schedule" />
                <asp:Button class="bitbtn btnsearch" Width="98px" runat="server" ID="btnPattern" Text="Pattern Shift" />
                <asp:Button class="bitbtn btnclear" Width="98px" runat="server" ID="btnClear" Text="Clear Shift" />
            </td>
            </tr>
         <tr>
                <td style="width:100px;text-align:right">Employee :</td>            
                <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter" 
                        AutoPostBack="True"/> 
                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbFilterName" Enabled="false" Width="225px"/>
                     
                      <asp:Button class="btngo" runat="server" ID="btnSearch" Text="..."  />

                </td>
             </tr>
      </table>      
                <br />  
          <asp:Panel runat="server" id="PnlHd">          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" PageSize="50" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
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
                  <asp:TemplateField HeaderText="Action" headerstyle-width="130" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>																
							</asp:TemplateField>
                  <asp:TemplateField HeaderText="Employee ID" HeaderStyle-Width="80" SortExpression="EmpNumb">
						<Itemtemplate>
							<asp:Label Runat="server" ID="EmpNumb" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:Label Runat="server" ID="EmpNumbEdit" text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>'>
							</asp:Label>
						</EditItemTemplate>								
				  </asp:TemplateField>
				  <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="160" SortExpression="EmpName">
						<Itemtemplate>
							<asp:Label Runat="server" ID="EmpName" text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:Label Runat="server" ID="EmpNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>'>
							</asp:Label>
						</EditItemTemplate>								
				  </asp:TemplateField>
				  <asp:TemplateField HeaderText="Job Title" HeaderStyle-Width="130" SortExpression="JobTitle">
						<Itemtemplate>
							<asp:Label Runat="server" ID="JobTitle" text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:Label Runat="server" ID="JobTitleEdit" text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>'>
							</asp:Label>
						</EditItemTemplate>								
				  </asp:TemplateField>
				  <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift001">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift001" text='<%# DataBinder.Eval(Container.DataItem, "Shift001") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift001Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift001") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift002">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift002" text='<%# DataBinder.Eval(Container.DataItem, "Shift002") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift002Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift002") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift003">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift003" text='<%# DataBinder.Eval(Container.DataItem, "Shift003") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift003Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift003") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift004">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift004" text='<%# DataBinder.Eval(Container.DataItem, "Shift004") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift004Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift004") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift005">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift005" text='<%# DataBinder.Eval(Container.DataItem, "Shift005") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift005Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift005") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift006">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift006" text='<%# DataBinder.Eval(Container.DataItem, "Shift006") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift006Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift006") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift007">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift007" text='<%# DataBinder.Eval(Container.DataItem, "Shift007") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift007Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift007") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift008">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift008" text='<%# DataBinder.Eval(Container.DataItem, "Shift008") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift008Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift008") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift009">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift009" text='<%# DataBinder.Eval(Container.DataItem, "Shift009") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift009Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift009") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift010">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift010" text='<%# DataBinder.Eval(Container.DataItem, "Shift010") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift010Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift010") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift011">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift011" text='<%# DataBinder.Eval(Container.DataItem, "Shift011") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift011Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift011") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift012">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift012" text='<%# DataBinder.Eval(Container.DataItem, "Shift012") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift012Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift012") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift013">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift013" text='<%# DataBinder.Eval(Container.DataItem, "Shift013") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift013Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift013") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift014">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift014" text='<%# DataBinder.Eval(Container.DataItem, "Shift014") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift014Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift014") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift015">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift015" text='<%# DataBinder.Eval(Container.DataItem, "Shift015") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift015Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift015") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift016">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift016" text='<%# DataBinder.Eval(Container.DataItem, "Shift016") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift016Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift016") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift017">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift017" text='<%# DataBinder.Eval(Container.DataItem, "Shift017") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift017Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift017") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift018">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift018" text='<%# DataBinder.Eval(Container.DataItem, "Shift018") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift018Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift018") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift019">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift019" text='<%# DataBinder.Eval(Container.DataItem, "Shift019") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift019Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift019") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift020">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift020" text='<%# DataBinder.Eval(Container.DataItem, "Shift020") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift020Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift020") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift021">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift021" text='<%# DataBinder.Eval(Container.DataItem, "Shift021") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift021Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift021") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift022">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift022" text='<%# DataBinder.Eval(Container.DataItem, "Shift022") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift022Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift022") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift023">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift023" text='<%# DataBinder.Eval(Container.DataItem, "Shift023") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift023Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift023") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift024">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift024" text='<%# DataBinder.Eval(Container.DataItem, "Shift024") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift024Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift024") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift025">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift025" text='<%# DataBinder.Eval(Container.DataItem, "Shift025") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift025Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift025") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift026">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift026" text='<%# DataBinder.Eval(Container.DataItem, "Shift026") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift026Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift026") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift027">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift027" text='<%# DataBinder.Eval(Container.DataItem, "Shift027") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift027Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift027") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift028">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift028" text='<%# DataBinder.Eval(Container.DataItem, "Shift028") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift028Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift028") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift029">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift029" text='<%# DataBinder.Eval(Container.DataItem, "Shift029") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift029Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift029") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift030">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift030" text='<%# DataBinder.Eval(Container.DataItem, "Shift030") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift030Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift030") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift031">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift031" text='<%# DataBinder.Eval(Container.DataItem, "Shift031") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift031Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift031") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift032">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift032" text='<%# DataBinder.Eval(Container.DataItem, "Shift032") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift032Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift032") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift033">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift033" text='<%# DataBinder.Eval(Container.DataItem, "Shift033") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift033Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift033") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift034">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift034" text='<%# DataBinder.Eval(Container.DataItem, "Shift034") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift034Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift034") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift035">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift035" text='<%# DataBinder.Eval(Container.DataItem, "Shift035") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift035Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift035") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift036">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift036" text='<%# DataBinder.Eval(Container.DataItem, "Shift036") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift036Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift036") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift037">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift037" text='<%# DataBinder.Eval(Container.DataItem, "Shift037") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift037Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift037") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift038">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift038" text='<%# DataBinder.Eval(Container.DataItem, "Shift038") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift038Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift038") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift039">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift039" text='<%# DataBinder.Eval(Container.DataItem, "Shift039") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift039Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift039") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift040">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift040" text='<%# DataBinder.Eval(Container.DataItem, "Shift040") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift040Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift040") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift041">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift041" text='<%# DataBinder.Eval(Container.DataItem, "Shift041") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift041Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift041") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift042">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift042" text='<%# DataBinder.Eval(Container.DataItem, "Shift042") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift042Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift042") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift043">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift043" text='<%# DataBinder.Eval(Container.DataItem, "Shift043") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift043Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift043") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift044">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift044" text='<%# DataBinder.Eval(Container.DataItem, "Shift044") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift044Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift044") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift045">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift045" text='<%# DataBinder.Eval(Container.DataItem, "Shift045") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift045Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift045") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift046">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift046" text='<%# DataBinder.Eval(Container.DataItem, "Shift046") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift046Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift046") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift047">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift047" text='<%# DataBinder.Eval(Container.DataItem, "Shift047") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift047Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift047") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift048">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift048" text='<%# DataBinder.Eval(Container.DataItem, "Shift048") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift048Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift048") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift049">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift049" text='<%# DataBinder.Eval(Container.DataItem, "Shift049") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift049Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift049") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift050">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift050" text='<%# DataBinder.Eval(Container.DataItem, "Shift050") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift050Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift050") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift051">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift051" text='<%# DataBinder.Eval(Container.DataItem, "Shift051") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift051Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift051") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift052">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift052" text='<%# DataBinder.Eval(Container.DataItem, "Shift052") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift052Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift052") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift053">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift053" text='<%# DataBinder.Eval(Container.DataItem, "Shift053") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift053Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift053") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift054">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift054" text='<%# DataBinder.Eval(Container.DataItem, "Shift054") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift054Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift054") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift055">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift055" text='<%# DataBinder.Eval(Container.DataItem, "Shift055") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift055Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift055") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift056">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift056" text='<%# DataBinder.Eval(Container.DataItem, "Shift056") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift056Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift056") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift057">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift057" text='<%# DataBinder.Eval(Container.DataItem, "Shift057") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift057Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift057") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift058">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift058" text='<%# DataBinder.Eval(Container.DataItem, "Shift058") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift058Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift058") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift059">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift059" text='<%# DataBinder.Eval(Container.DataItem, "Shift059") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift059Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift059") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift060">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift060" text='<%# DataBinder.Eval(Container.DataItem, "Shift060") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift060Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift060") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift061">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift061" text='<%# DataBinder.Eval(Container.DataItem, "Shift061") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift061Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift061") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift062">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift062" text='<%# DataBinder.Eval(Container.DataItem, "Shift062") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift062Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift062") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift063">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift063" text='<%# DataBinder.Eval(Container.DataItem, "Shift063") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift063Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift063") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift064">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift064" text='<%# DataBinder.Eval(Container.DataItem, "Shift064") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift064Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift064") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift065">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift065" text='<%# DataBinder.Eval(Container.DataItem, "Shift065") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift065Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift065") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift066">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift066" text='<%# DataBinder.Eval(Container.DataItem, "Shift066") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift066Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift066") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift067">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift067" text='<%# DataBinder.Eval(Container.DataItem, "Shift067") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift067Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift067") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift068">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift068" text='<%# DataBinder.Eval(Container.DataItem, "Shift068") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift068Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift068") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift069">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift069" text='<%# DataBinder.Eval(Container.DataItem, "Shift069") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift069Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift069") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift070">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift070" text='<%# DataBinder.Eval(Container.DataItem, "Shift070") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift070Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift070") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift071">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift071" text='<%# DataBinder.Eval(Container.DataItem, "Shift071") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift071Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift071") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift072">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift072" text='<%# DataBinder.Eval(Container.DataItem, "Shift072") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift072Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift072") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift073">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift073" text='<%# DataBinder.Eval(Container.DataItem, "Shift073") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift073Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift073") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift074">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift074" text='<%# DataBinder.Eval(Container.DataItem, "Shift074") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift074Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift074") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift075">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift075" text='<%# DataBinder.Eval(Container.DataItem, "Shift075") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift075Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift075") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift076">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift076" text='<%# DataBinder.Eval(Container.DataItem, "Shift076") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift076Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift076") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift077">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift077" text='<%# DataBinder.Eval(Container.DataItem, "Shift077") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift077Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift077") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift078">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift078" text='<%# DataBinder.Eval(Container.DataItem, "Shift078") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift078Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift078") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift079">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift079" text='<%# DataBinder.Eval(Container.DataItem, "Shift079") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift079Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift079") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift080">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift080" text='<%# DataBinder.Eval(Container.DataItem, "Shift080") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift080Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift080") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift081">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift081" text='<%# DataBinder.Eval(Container.DataItem, "Shift081") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift081Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift081") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift082">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift082" text='<%# DataBinder.Eval(Container.DataItem, "Shift082") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift082Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift082") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift083">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift083" text='<%# DataBinder.Eval(Container.DataItem, "Shift083") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift083Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift083") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift084">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift084" text='<%# DataBinder.Eval(Container.DataItem, "Shift084") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift084Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift084") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift085">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift085" text='<%# DataBinder.Eval(Container.DataItem, "Shift085") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift085Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift085") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift086">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift086" text='<%# DataBinder.Eval(Container.DataItem, "Shift086") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift086Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift086") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift087">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift087" text='<%# DataBinder.Eval(Container.DataItem, "Shift087") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift087Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift087") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift088">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift088" text='<%# DataBinder.Eval(Container.DataItem, "Shift088") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift088Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift088") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift089">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift089" text='<%# DataBinder.Eval(Container.DataItem, "Shift089") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift089Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift089") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift090">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift090" text='<%# DataBinder.Eval(Container.DataItem, "Shift090") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift090Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift090") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift091">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift091" text='<%# DataBinder.Eval(Container.DataItem, "Shift091") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift091Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift091") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift092">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift092" text='<%# DataBinder.Eval(Container.DataItem, "Shift092") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift092Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift092") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift093">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift093" text='<%# DataBinder.Eval(Container.DataItem, "Shift093") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift093Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift093") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift094">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift094" text='<%# DataBinder.Eval(Container.DataItem, "Shift094") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift094Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift094") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift095">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift095" text='<%# DataBinder.Eval(Container.DataItem, "Shift095") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift095Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift095") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift096">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift096" text='<%# DataBinder.Eval(Container.DataItem, "Shift096") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift096Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift096") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift097">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift097" text='<%# DataBinder.Eval(Container.DataItem, "Shift097") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift097Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift097") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift098">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift098" text='<%# DataBinder.Eval(Container.DataItem, "Shift098") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift098Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift098") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift099">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift099" text='<%# DataBinder.Eval(Container.DataItem, "Shift099") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift099Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift099") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift100">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift100" text='<%# DataBinder.Eval(Container.DataItem, "Shift100") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift100Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift100") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift101">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift101" text='<%# DataBinder.Eval(Container.DataItem, "Shift101") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift101Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift101") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift102">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift102" text='<%# DataBinder.Eval(Container.DataItem, "Shift102") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift102Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift102") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift103">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift103" text='<%# DataBinder.Eval(Container.DataItem, "Shift103") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift103Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift103") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift104">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift104" text='<%# DataBinder.Eval(Container.DataItem, "Shift104") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift104Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift104") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift105">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift105" text='<%# DataBinder.Eval(Container.DataItem, "Shift105") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift105Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift105") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift106">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift106" text='<%# DataBinder.Eval(Container.DataItem, "Shift106") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift106Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift106") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift107">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift107" text='<%# DataBinder.Eval(Container.DataItem, "Shift107") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift107Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift107") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift108">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift108" text='<%# DataBinder.Eval(Container.DataItem, "Shift108") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift108Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift108") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift109">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift109" text='<%# DataBinder.Eval(Container.DataItem, "Shift109") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift109Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift109") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift110">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift110" text='<%# DataBinder.Eval(Container.DataItem, "Shift110") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift110Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift110") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift111">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift111" text='<%# DataBinder.Eval(Container.DataItem, "Shift111") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift111Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift111") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift112">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift112" text='<%# DataBinder.Eval(Container.DataItem, "Shift112") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift112Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift112") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift113">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift113" text='<%# DataBinder.Eval(Container.DataItem, "Shift113") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift113Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift113") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift114">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift114" text='<%# DataBinder.Eval(Container.DataItem, "Shift114") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift114Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift114") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift115">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift115" text='<%# DataBinder.Eval(Container.DataItem, "Shift115") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift115Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift115") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift116">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift116" text='<%# DataBinder.Eval(Container.DataItem, "Shift116") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift116Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift116") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift117">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift117" text='<%# DataBinder.Eval(Container.DataItem, "Shift117") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift117Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift117") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift118">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift118" text='<%# DataBinder.Eval(Container.DataItem, "Shift118") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift118Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift118") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift119">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift119" text='<%# DataBinder.Eval(Container.DataItem, "Shift119") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift119Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift119") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift120">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift120" text='<%# DataBinder.Eval(Container.DataItem, "Shift120") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift120Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift120") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift121">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift121" text='<%# DataBinder.Eval(Container.DataItem, "Shift121") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift121Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift121") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift122">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift122" text='<%# DataBinder.Eval(Container.DataItem, "Shift122") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift122Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift122") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift123">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift123" text='<%# DataBinder.Eval(Container.DataItem, "Shift123") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift123Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift123") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift124">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift124" text='<%# DataBinder.Eval(Container.DataItem, "Shift124") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift124Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift124") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift125">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift125" text='<%# DataBinder.Eval(Container.DataItem, "Shift125") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift125Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift125") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift126">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift126" text='<%# DataBinder.Eval(Container.DataItem, "Shift126") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift126Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift126") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift127">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift127" text='<%# DataBinder.Eval(Container.DataItem, "Shift127") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift127Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift127") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift128">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift128" text='<%# DataBinder.Eval(Container.DataItem, "Shift128") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift128Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift128") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift129">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift129" text='<%# DataBinder.Eval(Container.DataItem, "Shift129") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift129Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift129") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift130">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift130" text='<%# DataBinder.Eval(Container.DataItem, "Shift130") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift130Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift130") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift131">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift131" text='<%# DataBinder.Eval(Container.DataItem, "Shift131") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift131Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift131") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift132">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift132" text='<%# DataBinder.Eval(Container.DataItem, "Shift132") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift132Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift132") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift133">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift133" text='<%# DataBinder.Eval(Container.DataItem, "Shift133") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift133Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift133") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift134">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift134" text='<%# DataBinder.Eval(Container.DataItem, "Shift134") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift134Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift134") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift135">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift135" text='<%# DataBinder.Eval(Container.DataItem, "Shift135") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift135Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift135") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift136">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift136" text='<%# DataBinder.Eval(Container.DataItem, "Shift136") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift136Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift136") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift137">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift137" text='<%# DataBinder.Eval(Container.DataItem, "Shift137") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift137Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift137") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift138">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift138" text='<%# DataBinder.Eval(Container.DataItem, "Shift138") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift138Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift138") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift139">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift139" text='<%# DataBinder.Eval(Container.DataItem, "Shift139") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift139Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift139") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift140">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift140" text='<%# DataBinder.Eval(Container.DataItem, "Shift140") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift140Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift140") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift141">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift141" text='<%# DataBinder.Eval(Container.DataItem, "Shift141") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift141Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift141") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift142">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift142" text='<%# DataBinder.Eval(Container.DataItem, "Shift142") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift142Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift142") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift143">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift143" text='<%# DataBinder.Eval(Container.DataItem, "Shift143") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift143Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift143") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift144">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift144" text='<%# DataBinder.Eval(Container.DataItem, "Shift144") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift144Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift144") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift145">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift145" text='<%# DataBinder.Eval(Container.DataItem, "Shift145") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift145Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift145") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift146">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift146" text='<%# DataBinder.Eval(Container.DataItem, "Shift146") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift146Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift146") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift147">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift147" text='<%# DataBinder.Eval(Container.DataItem, "Shift147") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift147Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift147") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift148">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift148" text='<%# DataBinder.Eval(Container.DataItem, "Shift148") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift148Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift148") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift149">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift149" text='<%# DataBinder.Eval(Container.DataItem, "Shift149") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift149Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift149") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift150">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift150" text='<%# DataBinder.Eval(Container.DataItem, "Shift150") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift150Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift150") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift151">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift151" text='<%# DataBinder.Eval(Container.DataItem, "Shift151") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift151Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift151") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift152">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift152" text='<%# DataBinder.Eval(Container.DataItem, "Shift152") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift152Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift152") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift153">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift153" text='<%# DataBinder.Eval(Container.DataItem, "Shift153") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift153Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift153") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift154">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift154" text='<%# DataBinder.Eval(Container.DataItem, "Shift154") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift154Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift154") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift155">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift155" text='<%# DataBinder.Eval(Container.DataItem, "Shift155") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift155Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift155") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift156">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift156" text='<%# DataBinder.Eval(Container.DataItem, "Shift156") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift156Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift156") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift157">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift157" text='<%# DataBinder.Eval(Container.DataItem, "Shift157") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift157Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift157") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift158">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift158" text='<%# DataBinder.Eval(Container.DataItem, "Shift158") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift158Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift158") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift159">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift159" text='<%# DataBinder.Eval(Container.DataItem, "Shift159") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift159Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift159") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift160">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift160" text='<%# DataBinder.Eval(Container.DataItem, "Shift160") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift160Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift160") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift161">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift161" text='<%# DataBinder.Eval(Container.DataItem, "Shift161") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift161Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift161") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift162">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift162" text='<%# DataBinder.Eval(Container.DataItem, "Shift162") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift162Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift162") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift163">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift163" text='<%# DataBinder.Eval(Container.DataItem, "Shift163") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift163Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift163") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift164">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift164" text='<%# DataBinder.Eval(Container.DataItem, "Shift164") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift164Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift164") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift165">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift165" text='<%# DataBinder.Eval(Container.DataItem, "Shift165") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift165Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift165") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift166">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift166" text='<%# DataBinder.Eval(Container.DataItem, "Shift166") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift166Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift166") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift167">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift167" text='<%# DataBinder.Eval(Container.DataItem, "Shift167") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift167Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift167") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift168">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift168" text='<%# DataBinder.Eval(Container.DataItem, "Shift168") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift168Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift168") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift169">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift169" text='<%# DataBinder.Eval(Container.DataItem, "Shift169") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift169Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift169") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift170">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift170" text='<%# DataBinder.Eval(Container.DataItem, "Shift170") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift170Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift170") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift171">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift171" text='<%# DataBinder.Eval(Container.DataItem, "Shift171") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift171Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift171") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift172">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift172" text='<%# DataBinder.Eval(Container.DataItem, "Shift172") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift172Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift172") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift173">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift173" text='<%# DataBinder.Eval(Container.DataItem, "Shift173") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift173Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift173") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift174">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift174" text='<%# DataBinder.Eval(Container.DataItem, "Shift174") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift174Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift174") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift175">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift175" text='<%# DataBinder.Eval(Container.DataItem, "Shift175") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift175Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift175") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift176">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift176" text='<%# DataBinder.Eval(Container.DataItem, "Shift176") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift176Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift176") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift177">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift177" text='<%# DataBinder.Eval(Container.DataItem, "Shift177") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift177Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift177") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift178">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift178" text='<%# DataBinder.Eval(Container.DataItem, "Shift178") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift178Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift178") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift179">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift179" text='<%# DataBinder.Eval(Container.DataItem, "Shift179") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift179Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift179") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="80" SortExpression="Shift180">
						<Itemtemplate>
							<asp:Label Runat="server" ID="Shift180" text='<%# DataBinder.Eval(Container.DataItem, "Shift180") %>'>
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="Shift180Edit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Shift180") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">									    
									</asp:DropDownList>	
						</EditItemTemplate>								
					</asp:TemplateField>
              </Columns>
          </asp:GridView>
          </div>   
          </asp:Panel>
          <asp:Panel runat="server" ID="pnlCopy" Visible="false">
      <table style="width: 583px">
        <tr>
            <td class="style6">Copy From</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlCopyFrom" CssClass="DropDownList" Width="100px" AutoPostBack="true" >
                    <asp:ListItem Selected="true">Employee</asp:ListItem>
                    <asp:ListItem>Organization</asp:ListItem>                    
                </asp:DropDownList>
            </td>                                
        </tr>                 
          <tr>
              <td class="style6">From </td>
              <td>:</td>
              <td>
                  <asp:DropDownList runat="server" ID="ddlDepartmentFrom" CssClass="DropDownList" Width="200px" />                
                  <asp:TextBox CssClass="TextBox" runat="server" 
                                ID="tbEmpNoFrom" MaxLength="15" Width="100px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpNameFrom" runat="server" CssClass="TextBoxR" Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                            <asp:Button ID="btnEmpFrom" runat="server" class="btngo" Text="..." />
              </td>
          </tr>
          <tr>
              <td class="style6">To</td>
              <td>:</td>
              <td><asp:DropDownList runat="server" ID="ddlDepartmentTo" CssClass="DropDownList" Width="200px" />                
              <asp:TextBox CssClass="TextBox" runat="server" 
                                ID="tbEmpNoTo" MaxLength="15" Width="100px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpNameTo" runat="server" CssClass="TextBoxR" Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                            <asp:Button ID="btnEmpTo" runat="server" class="btngo" Text="..." />
              </td>
          </tr>
      </table> 
      <br />
      <asp:Button ID="btnOKCopy" runat="server" class="bitbtndt btnok" Text="OK" /> &nbsp;    
      <asp:Button ID="btnCancelCopy" runat="server" class="bitbtndt btncancel" Text="Cancel" />  &nbsp;
        
      </asp:Panel>     
      
      <asp:Panel runat="server" ID="pnlPattern" Visible="false">
      <table>
      <tr>
      <td style="width:60%;">      
          <table style="width: 100%">
            <tr>
                <td class="style6">Choose Pattern</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" ID="ddlPattern" CssClass="DropDownList" Width="280px" AutoPostBack="true" >                    
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="tbPatternFormula" Visible ="false" CssClass="TextBox" Width="120px" Text="" />                                  
                </td>                         
            </tr>                 
              <tr>
                  <td class="style6">Where A</td>
                  <td>:</td>
                  <td>
                      <asp:DropDownList runat="server" ID="ddlShiftA" CssClass="DropDownList" Width="180px" AutoPostBack="true" />                                  
                  </td>
               </tr>
              <tr>
                  <td class="style1">B</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftB" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
              </tr>
              <tr>
                  <td class="style6">C</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftC" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
               </tr>
              <tr>
                  <td class="style1">D</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftD" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
              </tr>
              <tr>
                  <td class="style6">E</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftE" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
               </tr>
              <tr>
                  <td class="style1">F</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftF" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
              </tr>
              <tr>
                  <td class="style6">X</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" ID="ddlShiftX" CssClass="DropDownList" Width="180px" AutoPostBack="true"/>                                  
                  </td>
               </tr>
              <tr>
                  <td class="style1">&nbsp Start Index</td>
                  <td>:</td>
                  <td><asp:TextBox runat="server" ID="tbStartIndex" MaxLength="2" CssClass="TextBox" Width="20px" Text="1" AutoPostBack="true"/>                                  
                  </td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>
              <tr>
                <td colspan="3"></td>
              </tr>                                          
          </table> 
          </td>
            <td style="width:40%;">
                <table>
                <tr>
                <td>
                <asp:panel runat="server" ID="pnlHoliday">
                    <asp:CheckBox runat="server" ID="cbHolidayOff" Text="Check this to make holiday with shift 'OFF'" Checked="false" AutoPostBack="true"/>
                    <br />
                    <asp:GridView ID="GridHoliday" runat="server" AllowPaging="True" PageSize="10" 
                    CssClass="Grid" AutoGenerateColumns="false"> 
                    <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
					<RowStyle CssClass="GridItem" Wrap="false" />
					<AlternatingRowStyle CssClass="GridAltItem"/>
					<PagerStyle CssClass="GridPager" />
                    <Columns>              
                        <asp:BoundField HeaderText="Date" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" DataField="HolidayDate" HeaderStyle-Width="70" />                        
                        <asp:BoundField HeaderText="Description" DataField="HolidayName" HeaderStyle-Width="160" />                        
					</Columns>
					</asp:GridView>
                </asp:panel>
                </td>
                </tr>
                </table>
            </td>
      </tr>
      </table>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridSimulasi" runat="server" AllowPaging="False" AllowSorting="true" PageSize="1" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Shift" DataField="Shift001" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift002" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift003" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift004" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift005" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift006" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift007" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift008" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift009" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift010" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift011" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift012" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift013" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift014" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift015" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift016" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift017" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift018" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift019" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift020" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift021" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift022" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift023" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift024" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift025" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift026" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift027" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift028" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift029" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift030" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift031" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift032" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift033" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift034" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift035" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift036" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift037" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift038" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift039" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift040" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift041" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift042" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift043" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift044" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift045" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift046" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift047" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift048" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift049" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift050" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift051" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift052" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift053" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift054" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift055" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift056" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift057" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift058" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift059" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift060" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift061" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift062" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift063" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift064" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift065" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift066" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift067" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift068" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift069" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift070" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift071" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift072" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift073" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift074" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift075" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift076" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift077" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift078" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift079" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift080" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift081" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift082" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift083" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift084" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift085" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift086" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift087" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift088" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift089" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift090" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift091" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift092" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift093" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift094" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift095" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift096" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift097" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift098" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift099" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift100" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift101" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift102" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift103" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift104" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift105" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift106" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift107" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift108" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift109" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift110" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift111" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift112" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift113" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift114" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift115" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift116" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift117" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift118" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift119" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift120" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift121" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift122" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift123" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift124" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift125" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift126" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift127" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift128" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift129" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift130" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift131" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift132" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift133" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift134" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift135" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift136" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift137" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift138" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift139" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift140" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift141" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift142" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift143" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift144" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift145" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift146" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift147" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift148" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift149" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift150" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift151" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift152" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift153" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift154" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift155" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift156" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift157" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift158" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift159" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift160" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift161" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift162" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift163" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift164" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift165" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift166" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift167" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift168" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift169" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift170" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift171" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift172" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift173" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift174" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift175" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift176" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift177" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift178" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift179" HeaderStyle-Width="50"  />
				  <asp:BoundField HeaderText="Shift" DataField="Shift180" HeaderStyle-Width="50"  />
				  
              </Columns>
          </asp:GridView>
          </div>
      <br />
      <asp:Button ID="btnOKPattern" runat="server" class="bitbtndt btnok" Text="OK" /> &nbsp;    
      <asp:Button ID="btnCancelPattern" runat="server" class="bitbtndt btncancel" Text="Cancel" />  &nbsp;
        
      </asp:Panel> 
      <asp:Panel runat="server" ID="pnlView" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridSchedule" runat="server" AllowPaging="True" AllowSorting="true" PageSize="30" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Organization" DataField="Department_Name" HeaderStyle-Width="180" SortExpression="Department_Name"  />
				  <asp:BoundField HeaderText="Employee ID" DataField="Emp_No" HeaderStyle-Width="110" SortExpression="Emp_No" />
				  <asp:BoundField HeaderText="Employee Name" DataField="Emp_Name" HeaderStyle-Width="200" SortExpression="Emp_Name" />
				  <asp:BoundField HeaderText="Schedule Date" DataField="ScheduleDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="60" SortExpression="ScheduleDate" />
				  <asp:BoundField HeaderText="Shift" DataField="ShiftName" HeaderStyle-Width="70" SortExpression="ShiftName" />
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>                   
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:SqlDataSource ID="dsShift" runat="server"                 
                SelectCommand="EXEC S_GetShift">
        </asp:SqlDataSource>
    </form>
</body>
</html>
