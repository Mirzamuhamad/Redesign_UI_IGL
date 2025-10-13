<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMTNOrder.aspx.vb" Inherits="Transaction_TrMTNOrder_TrMTNOrder" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
    
        function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lbjudul"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr">Request No</asp:ListItem>
                      <asp:ListItem Value="MachineName">Machine</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EstCompleteDate)">Est Complete</asp:ListItem>
                      <asp:ListItem Value="ProjectManagerName">Project Manager</asp:ListItem>                      
                      <asp:ListItem Value="PrepareByName">Prepare By</asp:ListItem>                      
                      <asp:ListItem Value="ApprovedByName">Approval By</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                 
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>       
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr">Request No</asp:ListItem>
                      <asp:ListItem Value="MachineName">Machine</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EstCompleteDate)">Est Complete</asp:ListItem>
                      <asp:ListItem Value="ProjectManagerName">Project Manager</asp:ListItem>                      
                      <asp:ListItem Value="PrepareByName">Prepare By</asp:ListItem>                      
                      <asp:ListItem Value="ApprovedByName">Approval By</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>       
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>       
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />       
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference">
                  <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate">
                  <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="120px" HeaderText="Request No" SortExpression="ReffNmbr" />
                  <asp:BoundField DataField="MachineName" HeaderStyle-Width="150px" HeaderText="Machine" SortExpression="MachineName"/>
                  <asp:BoundField DataField="FgSubkon" HeaderStyle-Width="50px" HeaderText="Subkon" SortExpression="FgSubkon"/>
                  <asp:BoundField DataField="EstCompleteDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="100px" HeaderText="Est Complete" SortExpression="EstCompleteDate">
                  <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ProjectManagerName" HeaderStyle-Width="150px" HeaderText="Project Manager" SortExpression="ProjectManagerName"/>
                  <asp:BoundField DataField="PrepareByName" HeaderStyle-Width="150px" HeaderText="Prepare by" SortExpression="PrepareByName"/>
                  <asp:BoundField DataField="ApprovedByName" HeaderStyle-Width="150px" HeaderText="Approved By" SortExpression="ApprovedByName"/>
                  <asp:BoundField DataField="AcknowledByName" HeaderStyle-Width="150px" HeaderText="Acknowled By" SortExpression="AcknowledByName"/>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="350px" HeaderText="Remark" />
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>       
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>       
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
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
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>                        
        </tr>         
        <tr>
            <td><asp:Label runat="server" ID="lbReffType" Text = "Request No"></asp:Label>
            </td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbReffNmbr" Enabled="false" Width="130px"/> 
                <asp:Button class="bitbtn btngo" runat="server" ID="btnReffNmbr" Text="..."/>
                &nbsp &nbsp
                <asp:Button class="bitbtn btnsearch" Width="100px" runat="server" ID="btnGetData" Text="Get Data" ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Machine</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlMachine" Enabled="false" Width="180px" /> 
            </td>
        </tr>
        <tr>
            <td>Subkon</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFgSubkon" Width="50px" ValidationGroup="Input"> 
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    <asp:ListItem>Y</asp:ListItem>                    
                </asp:DropDownList>            
            </td>
        </tr>
        <tr>
            <td>Estimate Complete</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbEstComplete" runat="server" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    
            </td>              
        </tr>        
        <tr>
            <td>Project Manager</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbProjectManager" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbProjectManagerName" Width="250px"/>    
                <asp:Button class="bitbtn btngo" runat="server" ID="btnProjectManager" Text="..." ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Prepare By</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPrepare" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbPrepareName" Width="250px"/>    
                <asp:Button class="bitbtn btngo" runat="server" ID="btnPrepare" Text="..." ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Acknowled By</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAcknowledBy" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbAcknowledByName" Width="250px"/>    
                <asp:Button class="bitbtn btngo" runat="server" ID="btnAcknowled" Text="..." ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Approval By</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbApprovalBy" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbApprovalByName" Width="250px"/>    
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApproval" Text="..." ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" ValidationGroup="Input" Width="350px" MaxLength ="255"/>            
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>       
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>                        
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>       
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>       
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>       
                                <asp:Button class="bitbtndt  btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>       
                            </EditItemTemplate>
                        </asp:TemplateField>                                                
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnClosing" runat="server" Class="bitbtndt btngetitem" 
                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="Closing" 
                                    Text="Closing" />
                                <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                <%--<asp:Button ID="btnDetailPO" runat="server" Class="bitbtndt btngetitem" 
                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="DetailPO" 
                                    Text="Detail PO" Width="70" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MaintenanceItem" HeaderStyle-Width="60px" HeaderText="Maintenance Item">
                        <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MaintenanceItemName" HeaderStyle-Width="180px" HeaderText="Maintenance Item Name">
                        <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Job" HeaderStyle-Width="60px" HeaderText="Job">
                        <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JobName" HeaderStyle-Width="180px" HeaderText="Job Name">
                        <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemSpec" HeaderStyle-Width="350px" HeaderText="ItemSpec">
                        <HeaderStyle Width="350px" />
                        </asp:BoundField>                                                
                        <asp:BoundField DataField="JobDescription" HeaderStyle-Width="350px" HeaderText="Job Description">
                        <HeaderStyle Width="350px" />
                        </asp:BoundField>                                                
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="350px" HeaderText="Remark">
                        <HeaderStyle Width="350px" />
                        </asp:BoundField>                                                                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />       
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:Label ID="lbMaintenanceItem" runat="server" Text="Maintenance Item" /></td>
                    <td>:</td>                    
                    <td><asp:DropDownList ID="ddlMaintenanceItem" runat="server" CssClass="DropDownList" Width="310px" AutoPostBack="true" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnMaintenanceItem" Text="..." />    
                        <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbJob" runat="server" Text="Job" /></td>
                    <td>:</td>                    
                    <td><asp:DropDownList ID="ddlJob" runat="server" CssClass="DropDownList" Width="310px" AutoPostBack="true" />
                        <asp:Label ID="Label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbSpec" runat="server" Text="Item Specification" /></td>
                    <td>:</td>                    
                    <td><asp:TextBox ID="tbitemSpec" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" MaxLength="255" Width="350px"/>                        
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label1" runat="server" Text="Job Description" /></td>
                    <td>:</td>                    
                    <td><asp:TextBox ID="tbJobDesc" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" MaxLength="255" Width="350px"/>                        
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" MaxLength="255" Width="350px"/>
                    </td>
                </tr>                         
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                              
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                              
            <br />
       </asp:Panel> 
       <br />          
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px"/>                              
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                              
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                              
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>                              
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
    </body>
</html>
