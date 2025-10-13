<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrQCRR.Aspx.vb" Inherits="TrQCRR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        
        function setformat()
        {
         var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,"");             
         var QtySS = document.getElementById("tbQtySS").value.replace(/\$|\,/g,"");             
         try
         {                       
            document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');            
            document.getElementById("tbQtySS").value = setdigit(QtySS,'<%=ViewState("DigitQty")%>');            
        }catch (err){
            alert(err.description);
          }      
        }        
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">QC Incoming RR</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="TransNmbr">Request No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Request Date</asp:ListItem>
                      <asp:ListItem Value="RequestType">Request Type</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="DeptName">Department</asp:ListItem>
                      <asp:ListItem Value="RequestBy">Request By</asp:ListItem>                      
                      <asp:ListItem Value="Product">Product</asp:ListItem>                      
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
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
                      <asp:ListItem Selected="True" Value="TransNmbr">Request No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="RequestType">Request Type</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="DeptName">Department</asp:ListItem>
                      <asp:ListItem Value="RequestBy">Request By</asp:ListItem>                      
                      <asp:ListItem Value="Product">Product</asp:ListItem>                      
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
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
                              <%--<asp:ListItem Text="Print" />
                              <asp:ListItem Text="Print Schedule" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Request No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="InspectionType" HeaderStyle-Width="30px" SortExpression="InspectionType" HeaderText="Type"></asp:BoundField>
                  <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="200px" SortExpression="ReffNmbr" HeaderText="Refference No."></asp:BoundField>
                  <asp:BoundField DataField="InspectedPartyName" HeaderStyle-Width="200px" SortExpression="InspectedPartyName" HeaderText="Inspected Party"></asp:BoundField>
                  <asp:BoundField DataField="InspectedWrhsName" HeaderStyle-Width="200px" SortExpression="InspectedWrhsName" HeaderText="Inspected Warehouse"></asp:BoundField>
                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>	    
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
        <tr>
            <td>QC No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>            
        </tr>
        <tr>
            <td>QC Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>     
        
        <tr>
            <td>Inspection Type</td>
            <td>:</td>
            <td>    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlInspectionType" ValidationGroup="Input">
                      <asp:ListItem Selected="True" Value="First"></asp:ListItem>
                      <asp:ListItem Value="Repeated"></asp:ListItem>                                            
                    </asp:DropDownList> 
            </td>                  
        </tr>   
        <tr>
            <td>Reference Type</td>
            <td>:</td>
            <td>    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlReffType" ValidationGroup="Input" AutoPostBack="true">
                      <asp:ListItem Selected="True" Value="">Choose One</asp:ListItem>
                      <asp:ListItem >RR PO</asp:ListItem>
                      <asp:ListItem >RR Service</asp:ListItem>                                            
                    </asp:DropDownList> 
                    <asp:TextBox runat="server" Width = "120" ID="tbReffNmbr" CssClass="TextBox" Enabled="false" />
                    <asp:Button class="bitbtn btngo" runat="server" ID="btnReffNmbr" Text="..." />                    
                    <asp:Button ID="btnGetData" runat="server" class="bitbtn btngo" 
                    Text="Get Product" ValidationGroup="Input" Visible="false" Width="74px" />
            </td>                  
        </tr>           
        <tr>
            <td>Inspected Party</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" Width = "120" ID="tbParty" CssClass="TextBox" AutoPostBack="true" />
                <asp:TextBox runat="server" Width = "250" ID="tbPartyName" Enabled="false" CssClass="TextBox" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnParty" Text="..." ValidationGroup="Input" />                    
                <asp:Label ID="lbred0" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td>Inspected Wrhs</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlWrhs" CssClass="DropDownList" Width="200px" />
                <asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label></td>
        </tr>                                                 
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" Width="350px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />
                &nbsp;</td>
        </tr>
      </table>  
      
                    
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
       <%--<asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />--%>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
            <br />
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                    CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                    CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                    CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%></asp:TemplateField>                        
                            <asp:TemplateField>
                                <ItemTemplate>
                                <asp:Button ID="btnDetail" runat="server" class="bitbtndt btndetail" 
                                    CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                    CommandName="DetailPartial" Text="Detail" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" />
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px" 
                            HeaderText="Product Name" />
                        <asp:BoundField DataField="Location_Name" 
                            HeaderStyle-Width="120px" HeaderText="Location" 
                            SortExpression="Location_Name" />
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" 
                            HeaderText="Spesification" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-Width="50px" />
                        <asp:BoundField DataField="SamplingSize" HeaderText="Sampling Size" />
                        <asp:BoundField DataField="QtySS" HeaderText="Qty Sampling" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="IncomingDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Incoming Date" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="InspectionDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Inspection Date" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="InspectedBy" HeaderText="Inspected By" HeaderStyle-Width="200px"/>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                            HeaderText="Remark" />                        
                    </Columns>
                </asp:GridView>
            </div>
            <br />
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
            <tr>
                <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                <td>:</td>
                <td><asp:TextBox runat="server" ID="tbProdCode" CssClass="TextBox" AutoPostBack="true" />
                <asp:TextBox runat="server"  CssClass="TextBox"
                                ID="tbProdName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="260px"/>
                            <asp:Button Class="btngo" ID="btnProd" Text="..." runat="server" ValidationGroup="Input" />                                               
                            <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
                        </td>    
           </tr>                                                    
           <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td><asp:TextBox TextMode = "MultiLine" runat="server" 
                                CssClass="TextBoxMultiR" Width ="400px" ID="tbSpec" />                                              
                         </td>
                    </tr>
                    <tr>
                        <td>Location</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="DropDownList" Height="16px" Width="180px">                                
                            </asp:DropDownList>
                         </td>
                    </tr>
                    <tr>
                    <td>Incoming Date</td>
                    <td>:</td>
                    <td>
                    <BDP:BasicDatePicker ID="tbIncomingDate" runat="server" DateFormat="dd MMM yyyy" 
                            ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>            
                    </tr>  
                    <tr>
                    <td>Inspection Date</td>
                    <td>:</td>
                    <td>
                    <BDP:BasicDatePicker ID="tbInspectionDate" runat="server" DateFormat="dd MMM yyyy" 
                            ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>            
                    </tr>  
                    <tr>
                       <td>Qty</td>
                       <td>:</td>
                       <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" />
                            <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbUnit" Width="80px"/>
                       </td>                       
                    </tr>
                    <tr>
                       <td>Sampling Size</td>
                       <td>:</td>
                       <td>
                            <asp:DropDownList ID="ddlSamplingSize" runat="server" CssClass="DropDownList" Width="260px" AutoPostBack="true" >                                
                            </asp:DropDownList>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtySS" /> 
                            
                       </td>                       
                    </tr>
                    <tr>
                    <td>Inspection By</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbInspectionBy" runat="server" Width="200px" MaxLength="60" CssClass="TextBox" />
                    </td>            
                    </tr>                      
                    <tr>
                        <td>
                            Remark
                        </td>
                        <td>
                            :</td>
                        <td colspan="7">
                            <asp:TextBox ID="tbRemarkDt" runat="server" Width="350px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />
                        </td>
                    </tr>                
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       <br />     
       </asp:View>                       
            
            <asp:View ID="Tab3" runat="server">
                
                <asp:Label ID="Label5" runat="server" Text="Product :" />
                <asp:Label ID="lbProductDt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product oi" />
                <asp:Label ID="lbProductDtName2" runat="server" Font-Bold="true" ForeColor="Blue" Text="" />
                &nbsp &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="Label1" runat="server" Text="Qty :" />
                <asp:Label ID="lbQtyDt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="" />
                <asp:Label ID="lbSamplingSize" runat="server" Font-Bold="true" ForeColor="Blue" Text="" Visible="False" />
                <br />
                <asp:Panel ID="PnlPartial" runat="server">
                    <br />
                    <asp:Button ID="btnAddPartial" runat="server" class="bitbtndt btnadd" 
                        Text="Add" />
                    &nbsp;<asp:Button ID="btnBackPartial" runat="server" class="bitbtndt btnback" 
                        Text="Back" Width="60" />
                    <p />
                        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                            <asp:GridView ID="GridPartial" runat="server" AutoGenerateColumns="false" AllowPaging="true" 
                                    ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" 
                                                CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" 
                                                CommandName="Delete" Text="Delete"  OnClientClick="return confirm('Sure to delete this data?');"  />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtn btnupdatedt" 
                                                Text="Update" ValidationGroup="Input" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtn btncanceldt" 
                                                Text="Cancel" ValidationGroup="Input" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                    <asp:BoundField DataField="Defect_Name" HeaderText="Defect" HeaderStyle-Width="250px"/>
                                    <asp:BoundField DataField="Standard" HeaderText="Standard" HeaderStyle-Width="250px"/>
                                    <asp:BoundField DataField="Defect_Group_Name" HeaderText="Defect Group" HeaderStyle-Width="250px"/>
                                    <asp:BoundField DataField="Result" HeaderStyle-Width="250px" HeaderText="Result" />
                                    <asp:BoundField DataField="StatusOK" HeaderStyle-Width="50px" HeaderText="Status OK" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="350px"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <p />
                            <asp:Button ID="btnAddDt2Ke2" runat="server" class="bitbtndt btnadd" 
                                Text="Add" />
                            &nbsp;<asp:Button ID="btnbackDt3" runat="server" class="bitbtndt btnback" 
                                Text="Back" Width="60" />
                        </asp:Panel>
                        <p />
                        &nbsp;
                        <asp:Panel ID="pnlEditPartial" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            No</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:Label  ID="lbItemNo" runat="server" Width="60px"/>                                            
                                        </td>                                    
                                    </tr>
                                    <tr>
                                        <td>
                                            Defect</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbDefect" runat="server" CssClass="TextBox" Width="60px" AutoPostBack="true"/>
                                            <asp:TextBox ID="tbDefectName" runat="server" CssClass="TextBox" Width="180px" Enabled="false"/>
                                            <asp:Button Class="btngo" ID="btnDefect" Text="..." runat="server" ValidationGroup="Input" />                                               
                                            <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>                                    
                                    </tr>
                                    <tr>
                                        <td>Standard</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="tbStandard" runat="server" CssClass="TextBoxR" Width="250px" />
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>Defect Group</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="tbDefectGroup" runat="server" CssClass="TextBoxR" Width="250px" />
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>Result</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="tbResult" runat="server" CssClass="TextBoxMulti" Width="350px" MaxLength="255" TextMode="MultiLine" />
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>Status QC</td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusQC" runat="server" CssClass="DropDownList" Width="60px" >
                                                <asp:ListItem Selected ="True">Y</asp:ListItem>
                                                <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            Remark
                                        </td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Width="350px" />
                                        </td>
                                    </tr>
                                </table>
                                </td>
                <td style="vertical-align:top;">
                <br />
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" 
                                    validationgroup="Input" />									
                                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" 
                        Text="Cancel" />
                                <br />
                
              </asp:Panel>
               
            </asp:View>          
        </asp:MultiView>
      <br />
       	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
<%--    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
--%>    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>    
    </form>
    </body>
</html>
