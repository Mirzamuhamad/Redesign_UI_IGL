<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrIssueReq.aspx.vb" Inherits="Transaction_TrIssueReq_TrIssueReq" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Issue Request</title>
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

        function setformat()
        {
        
         try
         {           
        document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');        
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
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
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
    <div class="H1">Issue Request</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Department_Name">Department</asp:ListItem>
                      <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(RequireDate)">Require Date</asp:ListItem>
                      <asp:ListItem Value="Warehouse_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                          
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Department_Name">Department</asp:ListItem>
                  <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(RequireDate)">Require Date</asp:ListItem>
                  <asp:ListItem Value="Warehouse_Name">Warehouse</asp:ListItem>
                  <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                  <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                  <asp:ListItem Value="Remark">Remark</asp:ListItem>                  
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	     
            &nbsp &nbsp &nbsp                 
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />     
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="Reference" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                  <asp:BoundField DataField="Department_Name" HeaderStyle-Width="120px" HeaderText="Department" SortExpression="Department_Name">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="RequestBy" HeaderStyle-Width="120px" HeaderText="Request By" SortExpression="RequestBy">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="RequireDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Require Date" SortExpression="RequireDate"></asp:BoundField>                  
                  <asp:BoundField DataField="Warehouse_Name" HeaderStyle-Width="180px" HeaderText="Warehouse" SortExpression="Warehouse_Name">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Purpose" HeaderStyle-Width="120px" 
                      HeaderText="Purpose" SortExpression="Purpose">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Purpose_Name" HeaderStyle-Width="180px" 
                      HeaderText="Purpose Name" SortExpression="Purpose_Name">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	     
            &nbsp &nbsp &nbsp  

            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />          
                        
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
        <%--<tr>
            <td>
            <asp:ImageButton ID="btnGetDt" ValidationGroup="Input" runat="server"  
                    ImageUrl="../../Image/btnGetDataOn.png"
                    onmouseover="this.src='../../Image/btnGetDataOff.png';"
                    onmouseout="this.src='../../Image/btnGetDataOn.png';"
                    ImageAlign="AbsBottom" />             
            </td>            
        </tr>--%>
        <tr>
              <td>Department</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlDept" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                  </asp:DropDownList> 
                  <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>                 
              </td>
          </tr>
        <tr>
              <td>Request By</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbRequestBy" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="250px" />
              </td>
          </tr>
          
          <tr>
            <td>Require Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbRequireDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
            </td>                        
          </tr>      

          <tr>
              <td>Warehouse</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" ></asp:DropDownList>
                  <asp:TextBox ID="tbFgSubled" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Visible="False" />
              </td>
          </tr>
          <tr>
              <td>Subled</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbSubled" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbSubledName" runat="server" CssClass="TextBoxR" Enabled="False" Width="200px" />
                  <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" />                                                                            
                  
              </td>
          </tr>  
          <tr>
              <td>Purpose</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlPurpose" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                        <%--<asp:ListItem Selected="True">Sample</asp:ListItem>
                        <asp:ListItem>Sumbangan</asp:ListItem>
                        <asp:ListItem>Reject</asp:ListItem>
                        <asp:ListItem>Broken</asp:ListItem>
                        <asp:ListItem>Consumption</asp:ListItem>--%>
                  </asp:DropDownList>   
                  &nbsp&nbsp&nbsp&nbsp&nbsp
                  
                  <asp:Button Class="bitbtndt btngetitem" ID="btnGetDt" Text="Get Data" Width="80px" runat="server" ValidationGroup="Input"  />                                                                            
                  
              </td>
          </tr>        
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="250px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
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
                               <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
						       <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                            </ItemTemplate>
                            <EditItemTemplate>
                            		<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																											
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 													
                                
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnClosing" runat="server" CssClass="Button" Text="Closing"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="Closing" />
                                            <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                </ItemTemplate>
                        </asp:TemplateField>     
                        <asp:BoundField DataField="Product" HeaderStyle-Width="100px" HeaderText="Product" SortExpression="Product" ></asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HeaderStyle-Width="180px" SortExpression="Product_Name" ></asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" SortExpression="Specification" ></asp:BoundField>                        
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" ></asp:BoundField>
                        <asp:BoundField DataField="QtyClose" HeaderStyle-Width="80px" HeaderText="Qty Close" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="80px" HeaderText="Remark Close" ></asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" />
                                <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="200px" />
                                <asp:Button Class="btngo" ID="btn1" Text="..." runat="server" />                  
                             
                            </td>
                        </tr>
                        <tr>
                            <td>Spesification</td>
                            <td>:</td>
                            <td><asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" Enabled="False" Width="303px" /></td>
                        </tr>                
                        <tr>
                            <td>Qty</td>
                            <td>:</td>
                            <td><asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" />
                            </td>
                        </tr>                
                        <tr>
                            <td>
                                Unit</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" />
                            </td>
                        </tr>
                        <tr>                        
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
                	<asp:Panel runat="server" ID="PnlInfo" Visible="false" Height="100%" Width="100%">
                        <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Stock :"></asp:Label>
                        <br />
                        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                            <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
       </table>
            <br />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																											
									
			<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> 
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
   <%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%>
      <br />             
    </asp:Panel>               
    </div>   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />    
    </form>                            
    </body>
</html>
