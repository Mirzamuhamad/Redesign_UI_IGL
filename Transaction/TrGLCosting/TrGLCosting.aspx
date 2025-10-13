<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLCosting.aspx.vb" Inherits="TrGLCosting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<%@ Register TagPrefix="WaitDlg" TagName="WaitCtrl" Src="~/UserControl/Waiting.ascx" %>--%>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Costing Process</title>
    
        <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Function/Function.js" ></script> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
        <script type = "text/javascript">
            function DisableButton() {
                document.getElementById("<%=btnHome.ClientID %>").disabled = true;        
            }
            window.onbeforeunload = DisableButton;

            function ProgressCircle() {
                setTimeout(function() {
                    var modal = $('<div />');
                    modal.addClass("modal");
                    $('body').append(modal);
                    var loading = $(".loading");
                    loading.show();
                    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                    loading.css({ top: top, left: left });
                }, 200);
            }
            $('form').live("submit", function() {
                ProgressCircle();
            });
        </script>
    <style>
.modalBackground
{
	background-color: Gray;
	filter: alpha(opacity=80);
	opacity: 0.8;
}

.modalPopup
{
	/*background-color: #FFFFFF;*/
	background-color: #FFFFFF;	
	border-width: 1px;
	-moz-border-radius: 5px;
	border-style: solid;
	border-color: Gray;	
	min-width: 300px;
	max-width: 400px;
	min-height:150px;
	max-height:250px;	
	top:10px;
	left:50px;
}

.topHandle
{
	/*background-color: #97bae6;	*/
	background-color: #0066FF;	
	
}

.table
{
	padding:0;
	margin:0;
}
</style>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">        
        <%--<Services>            
        <asp:ServiceReference Path="~/App_WebReferences/Timbangan/Reference.svcmap" />         
      </Services>--%>
    </asp:ScriptManager>
    <div class="Content">
       <div class="H1">Costing Process</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="StartDate">Start Date</asp:ListItem>
                      <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />	
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
      </table>
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
                  <asp:ListItem Selected="True" Value="StartDate">Start Date</asp:ListItem>
                  <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
            
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />  
              <br />                       
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
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
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />                          
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="200px" SortExpression="StartDate" HeaderText="Start Date"></asp:BoundField>
                  <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="200px" SortExpression="EndDate" HeaderText="End Date"></asp:BoundField>                
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <br />
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
            
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
           <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Start Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
        </tr>              
        
        <tr>
            <td>End Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Label ID="lbType" Visible = "false" CssClass="TextBox" Text = "Product : " runat="server"/>            
            <asp:DropDownList ID="ddlTypeDt" Visible = "false" CssClass="DropDownList" runat="server" Width="300px" AutoPostBack = "true" />
              
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True" AllowPaging="true">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                         <%--Warehouse, WarehouseName, WrhsSubled, WrhsSubledName, Product, ProductName, 
                         BeginQty, InQty, OutQty, BalanceQty, BeginValue, InValue, OutValue, BalanceValue, 
                         PriceCost--%>
                        <asp:BoundField DataField="Warehouse" HeaderStyle-Width="120px" HeaderText="Warehouse" />
                        <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="WrhsSubledName" HeaderStyle-Width="100px" HeaderText="Wrhs Subled" />
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product" />
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="120px" HeaderText="Product Name" />
                        <asp:BoundField DataField="BeginQty" HeaderStyle-Width="80px" HeaderText="Begin Qty" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="InQty" HeaderStyle-Width="80px" HeaderText="In Qty" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="OutQty" HeaderStyle-Width="80px" HeaderText="Out Qty" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="BalanceQty" HeaderStyle-Width="80px" HeaderText="Balance Qty" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="BeginValue" HeaderStyle-Width="80px" HeaderText="Begin Value" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="InValue" HeaderStyle-Width="80px" HeaderText="In Value" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="OutValue" HeaderStyle-Width="80px" HeaderText="Out Value" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="BalanceValue" HeaderStyle-Width="80px" HeaderText="Balance Value" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="PriceCost" HeaderStyle-Width="80px" HeaderText="Price Cost" ItemStyle-HorizontalAlign="Right"/>
                    </Columns>
                </asp:GridView>
          </div>             
       </asp:Panel>             
       <br />          
         
         <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />  
         <asp:Button ID="btnCompute" Visible = "false" Text="Compute" CssClass="Button" runat="server"/>
         <asp:Button ID="btnComplete" Visible = "false" Text="Complete" CssClass="Button" runat="server"/>
         <asp:Button ID="btnUnComplete" Visible = "false" Text="Un-Complete" CssClass="Button" runat="server"/>
         <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									                       
        <br />        
        <%--<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        <%--<asp:Timer ID="Timer1" runat="server" Interval="2000" Enabled="false" />     --%>
        <%--<cc1:ModalPopupExtender ID="mpewait" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnShowPopup" PopupControlID="pnlWait" />
        <asp:Panel ID = "pnlWait" CssClass="modalPopup" runat="server">
            <WaitDlg:WaitCtrl ID="WaitOpen" runat = "server"/>
        </asp:Panel>--%>
        <asp:Label ID="lbMessageProcess" runat="server" ForeColor="Blue" />
        
    </asp:Panel>
    
    <asp:Panel ID="pnlView" runat="server" Visible="false">
        <asp:GridView ID="GridForView" runat="server" AutoGenerateColumns="true" 
            ShowFooter="True" AllowPaging="true" >
            <HeaderStyle CssClass="GridHeader" />
            <RowStyle CssClass="GridItem" Wrap="false" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <PagerStyle CssClass="GridPager" />
        </asp:GridView>        
    <br />
    <table>
        <tr>
            <td><asp:Label ID="lbMessage" runat="server" ForeColor="Blue" /> </td>            
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="Button" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="Button" />
            </td>            
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="Button" />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Button" />
               <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" Visible = false/>  
                <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>  
            </td>
        </tr>
    </table>
        
    
    </asp:Panel>    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">
     
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
</body>
</html>