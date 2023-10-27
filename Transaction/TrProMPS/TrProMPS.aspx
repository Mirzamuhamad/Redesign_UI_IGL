<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrProMPS.aspx.vb" Inherits="Transaction_TrProMPS_TrProMPS" %>

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
    <style type="text/css">
    #UpdatePanel1, #UpdatePanel2, #UpdateProgress1 { 
      border-right: gray 1px solid; border-top: gray 1px solid; 
      border-left: gray 1px solid; border-bottom: gray 1px solid;
    }
    #UpdatePanel1, #UpdatePanel2 { 
      width:200px; height:200px; position: relative;
      float: left; margin-left: 10px; margin-top: 10px;
     }
     #UpdateProgress1 {
      width: 400px; background-color: #FFC080; 
      bottom: 0%; left: 0px; position: absolute;
     }
        .style1
        {
            width: 172px;
        }
        .style3
        {
            width: 162px;
        }
    </style>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);
                var postBackElement;
                function InitializeRequest(sender, args) {
                    if (prm.get_isInAsyncPostBack()) {
                        args.set_cancel(true);
                    }
                    postBackElement = args.get_postBackElement();
                    if (postBackElement.id == 'ButtonTrigger') {
                        $get('UpdateProgress1').style.display = "block";
                    }
                }
                function EndRequest(sender, args) {
                    if (postBackElement.id == 'ButtonTrigger') {
                        $get('UpdateProgress1').style.display = "none";
                    }
                }
                function AbortPostBack() {
                    if (prm.get_isInAsyncPostBack()) {
                        prm.abortPostBack();
                    }
                }
            </script>
           <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2" >
            <ProgressTemplate >
              Update in progress...
              <input type="button" value="stop" onclick="AbortPostBack()" />
            </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button ID="ButtonTrigger" runat="server" Text="Refresh Panel 1" OnClick="Button_Click" />    
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">--%>
            <%--<ContentTemplate>
            <%=DateTime.Now.ToString() %> <br />
            The trigger for this panel 
            causes the UpdateProgress to be displayed
            even though the UpdateProgress is associated
            with panel 2.     
            <br />
            </ContentTemplate>--%>
            <%--<Triggers>
              <asp:AsyncPostBackTrigger ControlID="ButtonTrigger" />
            </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <%=DateTime.Now.ToString() %> <br />
            <asp:Button ID="Button2" runat="server" Text="Refresh Panel" OnClick="Button_Click"/>    
            </ContentTemplate>
            </asp:UpdatePanel>--%>
            
    
    
    
    <div class="Content">
    <div class="H1">Production Plan - MRP</div>
     <hr style="color:Blue" />        
      <table>
        <%--<tr>
            <td style="width:100px;text-align:right">Period :</td>         
            <td>
                <asp:TextBox ID="tbStartPeriod" runat="server" CssClass="TextBox" Width="90px" AutoPostBack ="true" />                
                <asp:Button class="bitbtn btngo" runat="server" ID="btnStartPeriod" Text="..." />
                <asp:TextBox ID="lbStartPeriod" runat="server" CssClass="TextBox" Enabled="false" Width="140px" />                
                &nbsp - &nbsp
                <asp:TextBox ID="tbEndPeriod" runat="server" CssClass="TextBox" Width="90px" AutoPostBack ="true" />                
                <asp:Button class="bitbtn btngo" runat="server" ID="btnEndPeriod" Text="..." />
                <asp:TextBox ID="lbEndPeriod" runat="server" CssClass="TextBox" Enabled="false" Width="140px" />
                <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                
                
            </td>  
            <td> - </td>
            <td><BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> </td>           
            <td>
                <asp:Button class="bitbtn btnpreview" Width="80px" runat="server" ID="btnProcess" Text="Process" />                
                <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnCreateMRP" Text="Create MRP" />                
            </td>
            </tr>--%>          
            <tr>
                <td style="text-align:right" colspan="2" class="style3">
                    <asp:Label ID="lbMessageProcess" runat="server" ForeColor="Blue" />
                    </td>         
            </tr>
            <tr>
                <td>
                    Periode :
                </td>
                <td class="style3">
                    <BDP:BasicDatePicker ID="tbPeriode" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                        ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="true">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>
            </tr>          
        </table>      
          <br />
        <table>
            <tr>
                <td>
                    <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                        StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                        StaticEnableDefaultPopOutImage="False">
                        <Items>
                            <asp:MenuItem Text="Production Plan" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="M R P" Value="1"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </td>
                <td class="style1">
                </td>
                <td>
                    <%--<asp:Panel ID="pnlExp1" runat="server">--%>
                    <asp:Button class="bitbtn btnpreview" Width="105px" runat="server" ID="btnImport" Text="Export To Excel" />
                    <%--</asp:Panel>--%>
                </td>
                <%--<td>
                     <asp:Panel ID="pnlExp2" runat="server">
                        <asp:Button class="bitbtn btnpreview" Width="105px" runat="server" ID="btnImport2" Text="Import To Excel" /> 
                    </asp:Panel>
                </td>--%>
            </tr>
        </table>
        
        <hr />
        <asp:Panel ID="pnlData" runat="server" Visible="false">
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
        <asp:View ID="Tab1" runat="server">
          <asp:Panel runat="server" id="pnlResult">          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" PageSize="30" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Work Center" DataField="WorkCtr" HeaderStyle-Width="150" SortExpression="WorkCtr"  />                 
                  <asp:BoundField HeaderText="Product" DataField="Product" HeaderStyle-Width="120" SortExpression="Product"  />
                  <asp:BoundField HeaderText="Product Name" DataField="ProductName" HeaderStyle-Width="250" SortExpression="ProductName"  />
                  <asp:BoundField HeaderText="Specification" DataField="Specification" HeaderStyle-Width="250" SortExpression="Specification"  />
                  <asp:BoundField HeaderText="SO Out" DataField="QtySO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="IO out" DataField="QtyIO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Buffer" DataField="QtyBF" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <%--<asp:BoundField HeaderText="PR Out" DataField="QtyRequest" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80" />--%>
                  <asp:BoundField HeaderText="On Hand" DataField="QtyOnHand" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Qty NR" DataField="QtyNR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="NR WO" DataField="QtyNRWO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="NR Non WO" DataField="QtyNRNonWO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Unit" DataField="Unit" HeaderStyle-Width="50" />
                  <%--<asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Date" SortExpression="StartDate"/>--%>                  
                  <%--<asp:BoundField HeaderText="Done WO" DataField="DoneWO" HeaderStyle-Width="30" />                  --%>
              </Columns>
          </asp:GridView>
          
          <asp:GridView ID="GridView2" runat="server"  AllowPaging="false" 
            CssClass="Grid" AutoGenerateColumns="false" Visible="true"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Work Center" DataField="WorkCtr" HeaderStyle-Width="150" SortExpression="WorkCtr"  />                 
                  <asp:BoundField HeaderText="Product" DataField="Product" HeaderStyle-Width="120" SortExpression="Product"  />
                  <asp:BoundField HeaderText="Product Name" DataField="ProductName" HeaderStyle-Width="250" SortExpression="ProductName"  />
                  <asp:BoundField HeaderText="Specification" DataField="Specification" HeaderStyle-Width="250" SortExpression="Specification"  />
                  <asp:BoundField HeaderText="Product Group" DataField="ProductGroup" HeaderStyle-Width="250" SortExpression="ProductName"  />
                  <asp:BoundField HeaderText="Product Sub Group" DataField="ProductSubGroup" HeaderStyle-Width="250" SortExpression="ProductName"  />
                  <asp:BoundField HeaderText="SO Out" DataField="QtySO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="IO out" DataField="QtyIO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Buffer" DataField="QtyBF" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <%--<asp:BoundField HeaderText="PR Out" DataField="QtyRequest" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />--%>
                  <%--<asp:BoundField HeaderText="WO Out" DataField="QtyReceipt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />--%>
                  <asp:BoundField HeaderText="On Hand" DataField="QtyOnHand" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Qty NR" DataField="QtyNR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="NR WO" DataField="QtyNRWO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="NR Non WO" DataField="QtyNRNonWO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="Unit" DataField="Unit" HeaderStyle-Width="50" />
                  <%--<asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Date" SortExpression="StartDate"/>--%>                  
                  <%--<asp:BoundField HeaderText="Done WO" DataField="DoneWO" HeaderStyle-Width="30" />                  --%>
              </Columns>
          </asp:GridView>
          
          </div>   
          </asp:Panel>
          </asp:View>           
        <asp:View ID="Tab2" runat="server">                  
          <asp:Panel ID="pnlViewMRP" runat="server">
              <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnCreatePR" Text="Create PR" />                
              <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnCreateWO" Text="Create WO" Visible="false"/>                  
              <br />
              <br />              
              <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridMRP" runat="server" AllowPaging="True" AllowSorting="true" PageSize="30" 
                CssClass="Grid" AutoGenerateColumns="false" Width="100%"> 
                  <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						    <RowStyle CssClass="GridItem" Wrap="false" />
						    <AlternatingRowStyle CssClass="GridAltItem"/>
						    <PagerStyle CssClass="GridPager" />
                  <Columns> 
                      <asp:BoundField HeaderText="Work Center" DataField="WorkCtr" HeaderStyle-Width="120"/>                 
                      <asp:BoundField HeaderText="Product" DataField="Product" HeaderStyle-Width="120" />
                      <asp:BoundField HeaderText="Product Name" DataField="ProductName" HeaderStyle-Width="250" />
                      <asp:BoundField HeaderText="Specification" DataField="Specification" HeaderStyle-Width="200" />
                      <asp:BoundField HeaderText="Qty Gross" DataField="QtySO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />                  
                      <asp:BoundField HeaderText="Buffer" DataField="QtyBF" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="PR Out" DataField="QtyPROut" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <%--<asp:BoundField HeaderText="WO Out" DataField="QtyWOOut" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />--%>
                      <asp:BoundField HeaderText="On Hand" DataField="QtyOH" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="Qty NR" DataField="QtyNR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="Unit" DataField="Unit" HeaderStyle-Width="50" />                                                 
                  </Columns>
              </asp:GridView>
              
              <asp:GridView ID="GridView3" runat="server" AllowPaging="false" Visible="true"
                CssClass="Grid" AutoGenerateColumns="false" Width="100%"> 
                  <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						    <RowStyle CssClass="GridItem" Wrap="false" />
						    <AlternatingRowStyle CssClass="GridAltItem"/>
						    <PagerStyle CssClass="GridPager" />
                  <Columns> 
                      <asp:BoundField HeaderText="Work Center" DataField="WorkCtr" HeaderStyle-Width="120"/>                 
                      <asp:BoundField HeaderText="Product" DataField="Product" HeaderStyle-Width="120" />
                      <asp:BoundField HeaderText="Product Name" DataField="ProductName" HeaderStyle-Width="250" />
                      <asp:BoundField HeaderText="Specification" DataField="Specification" HeaderStyle-Width="200" />
                      <asp:BoundField HeaderText="Product Group" DataField="Product_Group" HeaderStyle-Width="250" SortExpression="ProductName"  />
                    <asp:BoundField HeaderText="Product Sub Group" DataField="Product_SubGroup" HeaderStyle-Width="250" SortExpression="ProductName"  />
                    <asp:BoundField HeaderText="Qty Gross" DataField="QtySO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />                  
                      <asp:BoundField HeaderText="Buffer" DataField="QtyBF" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="PR Out" DataField="QtyPROut" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <%--<asp:BoundField HeaderText="WO Out" DataField="QtyWOOut" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />--%>
                      <asp:BoundField HeaderText="On Hand" DataField="QtyOH" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="Qty NR" DataField="QtyNR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="60" />
                      <asp:BoundField HeaderText="Unit" DataField="Unit" HeaderStyle-Width="50" />                                                 
                  </Columns>
              </asp:GridView>
              </div>                            
          </asp:Panel>
          <asp:Panel ID="pnlCreateMRP" runat="server">
              <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">  
              <table>
              <tr>
              <td>
              <asp:Label runat="server" ID="lb1" Text = "Work Center : "></asp:Label>
              <asp:DropDownList runat="server" ID="ddlWorkCtr" CssClass="DropDownList" AutoPostBack="true" Width="150">
              </asp:DropDownList>
              </td>
              <td>
              <asp:Label runat="server" ID="Label1" CssClass="DropDownList" Text = "Request To : "></asp:Label>
              <asp:DropDownList runat="server" ID="ddlToPurchase" Width="150">
                <asp:ListItem Selected="True" Value="Purchasing"></asp:ListItem>
                <asp:ListItem Value="Exim"></asp:ListItem>                                            
              </asp:DropDownList>
              </td>
              <td>
              <asp:Label runat="server" ID="Label2" Text = "Department : "></asp:Label>
              <asp:DropDownList runat="server" ID="ddlDept" CssClass="DropDownList" AutoPostBack="true" Width="150">
              </asp:DropDownList>
              </td>
              <td>
              <asp:Label runat="server" ID="Label3" Text = "Cost Center : "></asp:Label>
              <asp:DropDownList runat="server" ID="ddlCostCtr" CssClass="DropDownList" Width="150">
              </asp:DropDownList>
              </td>
              </tr>
              
              </table>
              
              
              
              
              <asp:Label runat="server" Id="lbGenerate" Text="Fill Qty NR greater than 0 will be generate PR"></asp:Label>
              &nbsp &nbsp &nbsp &nbsp &nbsp
              <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnGenerate" Text="Generate PR" />                
              <br />
              <br />
                <asp:GridView id="GridPR" runat="server" AutoGenerateColumns="False" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Product" HeaderStyle-Width="100px"  SortExpression="Product">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'/>
								</Itemtemplate>															
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="220px" SortExpression="ProductName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="250px" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						
							<asp:TemplateField HeaderText="Qty Gross" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="QtyGross" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyGross"))) %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Buffer" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="QtyBF" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyBF"))) %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>							
							<asp:TemplateField HeaderText="PR Out" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="QtyPROut" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyPROut"))) %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>							
							<%--<asp:TemplateField HeaderText="WO Out" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="QtyWOOut" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyWOOut"))) %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>			--%>
							<asp:TemplateField HeaderText="On Hand" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="QtyOH" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyOH"))) %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>											

							<asp:TemplateField HeaderText="Qty NR" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyNR">
								<Itemtemplate>
									<asp:TextBox runat="server" Width="100%" CssClass="TextBox" ID="QtyNR" Maxlength = "15" text='<%# String.Format("{0:#,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "QtyNR"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
    					</Columns>
                    </asp:GridView> 
                  </div>  
          </asp:Panel>
      </asp:View>                 
      </asp:MultiView> 
      </asp:Panel>
      <asp:Panel ID="pnlView" runat="server" Visible="false">
          <asp:GridView ID="GridForView" runat="server" AllowPaging="true" 
              AutoGenerateColumns="true" ShowFooter="True">
              <HeaderStyle CssClass="GridHeader" />
              <RowStyle CssClass="GridItem" Wrap="false" />
              <AlternatingRowStyle CssClass="GridAltItem" />
              <PagerStyle CssClass="GridPager" />
          </asp:GridView>
          <br />
          <table>
              <tr>
                  <td>
                      <asp:Label ID="lbMessage" runat="server" ForeColor="Blue" />
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Button ID="btnYes" runat="server" CssClass="Button" Text="Yes" />
                      <asp:Button ID="btnNo" runat="server" CssClass="Button" Text="No" />
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Button ID="btnReturn" runat="server" CssClass="Button" Text="Return" />
                  </td>
              </tr>
          </table>
        
    
    </asp:Panel>                      
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
