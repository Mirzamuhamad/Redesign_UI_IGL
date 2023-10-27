<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWorkOrder.aspx.vb" Inherits="Transaction_TrWorkOrder_TrWorkOrder" %>
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
        
         function setformat()
        {
         try
         {  
            document.getElementById("tbQtyM2").value = setdigit(document.getElementById("tbQtyM2").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                    
            document.getElementById("tbQtyRoll").value = setdigit(document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyFG").value = setdigit(document.getElementById("tbQtyFG").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                        
            
        }catch (err){
            alert(err.description);
          }      
        }
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
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
                      <%--<asp:ListItem Value="WOGroup">WO Group</asp:ListItem>--%>
                      <asp:ListItem Value="WorkCtr">WorkCtr</asp:ListItem>
                      <asp:ListItem Value="WorkCtr_Name">WorkCtr Name</asp:ListItem> 
                      <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr">Reff. Nmbr</asp:ListItem>
                      <asp:ListItem Value="ProductFg">Product FG Code</asp:ListItem>
                      <asp:ListItem Value="ProductFg_Name">Product FG Name</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                 
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>       
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" Visible="false" />
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
                      <%--<asp:ListItem Value="WOGroup">WO Group</asp:ListItem>--%>
                      <asp:ListItem Value="WorkCtr">WorkCtr</asp:ListItem>
                      <asp:ListItem Value="WorkCtr_Name">WorkCtr Name</asp:ListItem> 
                      <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr">Reff. Nmbr</asp:ListItem>
                      <asp:ListItem Value="ProductFg">Product FG Code</asp:ListItem>
                      <asp:ListItem Value="ProductFg_Name">Product FG Name</asp:ListItem>
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
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                              <%--<asp:ListItem Text="Photo" />
                              <asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />       
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"><HeaderStyle Width="120px" /></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>
                  <%--<asp:BoundField DataField="WOGroup" HeaderStyle-Width="120px" HeaderText="WO Group" SortExpression="WOGroup" />--%>
                  <asp:BoundField DataField="WorkCtr" HeaderStyle-Width="200px" SortExpression="WorkCtr" HeaderText="WorkCtr"></asp:BoundField>
                  <asp:BoundField DataField="WorkCtr_Name" HeaderStyle-Width="80px" SortExpression="WorkCtr_Name" HeaderText="WorkCtr Name"></asp:BoundField>
                  <asp:BoundField DataField="Purpose" HeaderStyle-Width="350px" HeaderText="Purpose" SortExpression="Purpose"/>
                  <asp:BoundField DataField="ReffType" HeaderStyle-Width="120px" HeaderText="Reff. Type" />
                  <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="200px" HeaderText="Reff. Nmbr" />
                  <asp:BoundField DataField="ProductFg" HeaderStyle-Width="120px" HeaderText="Product FG Code" />
                  <asp:BoundField DataField="ProductFg_Name" HeaderStyle-Width="200px" HeaderText="Product FG Name" />
                  <asp:BoundField DataField="SpecificationFG" HeaderStyle-Width="200px" HeaderText="Specification FG" />
                  <asp:BoundField DataField="Qty"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" HeaderText="Qty" />
                  <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" HeaderText="Unit" />
                  <asp:BoundField DataField="QtyRoll" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" HeaderText="Qty (Roll)" />
                  <asp:BoundField DataField="QtyM2"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" HeaderText="Qty (M2)" />  
                  <asp:BoundField DataField="BOMNo" HeaderStyle-Width="100px" HeaderText="BOM No" />                  
                  <asp:BoundField DataField="PercentWaste"  ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="350px" HeaderText="Waste (%)" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="350px" HeaderText="Remark" SortExpression="Remark"/>
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
            <td>Work Center</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlWorkCtr" runat="server" AutoPostBack="true" />
            <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>  
            <%--<td>WO Group</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlWOGroup" Width="80px" ValidationGroup="Input"> 
                    <asp:ListItem Selected="True">PE</asp:ListItem>
                    <asp:ListItem>TISSUE</asp:ListItem>                    
                </asp:DropDownList>            
            </td> --%>
            <td>Purpose</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlPurpose" Width="150px" ValidationGroup="Input"> 
                    <asp:ListItem Selected="True">Production</asp:ListItem>
                    <asp:ListItem>Trial</asp:ListItem>                    
                </asp:DropDownList>            
            </td>           
        </tr>       
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlReffType" Width="50px" AutoPostBack="true" ValidationGroup="Input"> 
                <asp:ListItem>IO</asp:ListItem>
                <asp:ListItem Selected="True">SO</asp:ListItem>                    
                </asp:DropDownList> 
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbReffNmbr" Enabled="false" Width="130px"/> 
                <asp:Button class="bitbtn btngo" runat="server" ID="btnReffNmbr" Text="..."/>
                            &nbsp &nbsp                
            </td>
                       
         </tr>
         <tr>
             <td>Product Finish Good</td>
             <td>:</td>
             <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbProductFG" Width="120px" Enabled="False" /> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbProductNameFG" Width="250px"/>                                
             </td>
         </tr>  
         <tr>
              <td>Specification</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbSpecificationFG" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" Enabled="False"  Width="350px" MaxLength ="255"/>            
              </td>
         </tr>
         <tr>
              <td>Qty 1</td>
              <td>:</td>
              <td><asp:TextBox CssClass="TextBox" runat="server" AutoPostBack="true" ValidationGroup="Input" ID="tbQtyFG" Width="80px"/> </td>                
              <td>Unit</td>
              <td>:</td>
              <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbUnitFG" Width="80px"/> </td>                
          </tr>  
          <tr>
              <td>Qty 2</td>
              <td>:</td>
              <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbQtyRoll" Width="80px"/> Roll
                  <asp:TextBox CssClass="TextBox" runat="server" Enabled="False" Visible="False" ID="tbUnitPerRoll" Width="80px"/> </td>                
              <td>Qty 3</td>
              <td>:</td>
              <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbQtyM2" Width="80px"/> M2
                  <asp:TextBox CssClass="TextBox" runat="server" Enabled="False" Visible="False" ID="tbUnitPerM2" Width="80px"/></td>                
                        
            </tr>
            <tr>
               <td>BOM No</td>
               <td>:</td>
               <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="true" ID="tbBOMNo" Width="120px" AutoPostBack="true" /> 
                   <asp:Button class="bitbtn btngo" runat="server" ID="btnBOM" Text="..." ValidationGroup="Input"/>
                   <asp:Label runat ="server" Text="*" ID="Label5" ForeColor="Red"/>
                   <asp:Label runat="server" ID="lbOutputType" Visible="false"></asp:Label>
               </td>
                <td>Waste (%)</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbWaste" Width="80px"/> 
                </td>  
        </tr>
          
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" ValidationGroup="Input" Width="350px" MaxLength ="255"/>            
            <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Item" ValidationGroup="Input"  />     
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add"/>       
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>                        
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>                                       
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>       
                                <asp:Button class="bitbtndt  btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>       
                            </EditItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>       
                            </ItemTemplate>
                        </asp:TemplateField>                                              
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnClosing" runat="server" Class="bitbtndt btngetitem" CommandArgument="<%# Container.DataItemIndex %>" CommandName="Closing" Text="Closing" />                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detail">
                            <ItemTemplate>
                                <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btngetitem" CommandArgument="<%# Container.DataItemIndex %>" CommandName="DetailMaterial" Text="Detail" Width="70"/>    
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderText="No" />
                        <asp:BoundField DataField="LotNo" HeaderStyle-Width="100px" HeaderText="Lot No" />
                        <asp:BoundField DataField="FormulaName" HeaderStyle-Width="120px" HeaderText="Formula" />
                        <asp:BoundField DataField="Process" HeaderStyle-Width="120px" HeaderText="Process Code" />
                        <asp:BoundField DataField="Process_Name" HeaderStyle-Width="200px" HeaderText="Process Name" />                        
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" HeaderText="Product Output Code" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="200px" HeaderText="Product Output Name" />
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" HeaderText="Qty" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" HeaderText="Unit" />
                        <asp:BoundField DataField="ProductionDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Prod. Date" SortExpression="ProductionDate"></asp:BoundField>                      
                        <asp:BoundField DataField="Machine" HeaderStyle-Width="100px" HeaderText="Machine" />
                        <asp:BoundField DataField="Machine_Name" HeaderStyle-Width="100px" HeaderText="Machine Name" />
                        <asp:BoundField DataField="ChartingPosition" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" HeaderText="Charting Position #1" />
                        <asp:BoundField DataField="DesignPrinting" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" HeaderText="Charting Position #2" />                         
                        <asp:BoundField DataField="DODate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="DO Date" SortExpression="DODate"></asp:BoundField>                      
                        <asp:BoundField DataField="FgSubkon" HeaderStyle-Width="60px" HeaderText="Subkon" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />           
                        <asp:BoundField DataField="DoneClosing" HeaderStyle-Width="30px" HeaderText="Done Closing" />           
                        <asp:BoundField DataField="QtyClose" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" HeaderText="Qty Close" />
                        <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="200px" HeaderText="Remark Close" />           
                        <asp:BoundField DataField="DateClose" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date Close" SortExpression="DateClose"></asp:BoundField>                      
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" />       
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td> 
                        <td>Lot No</td> 
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbLotNo" Width="149px"/> 
                        </td>          
                    </tr> 
                      
                    <tr>
                        <td>Process</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbProcessCode" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbProcessName" Width="250px"/>    
                            <asp:Button class="bitbtn btngo" runat="server" ID="btnProcess" Text="..." ValidationGroup="Input"/>
                            <asp:Label runat ="server" Text="*" ID="Label3" ForeColor="Red"/>
                        </td>
                    </tr> 
                    <tr>
                        <td>Formula</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbFormulaCode" Width="120px"/> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbFormulaName" Width="250px"/>                                
                        </td>
                    </tr>   
                    <tr>
                        <td>Product Output</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbProductCode" Width="120px" Enabled="False" /> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbProductName" Width="250px"/>                               
                        </td>
                    </tr>  
                    <%--<tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" Enabled="False"  Width="350px" MaxLength ="255"/>            
                        </td>
                    </tr>--%>
                    <tr>
                        <td>Qty 1</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" AutoPostBack="true" ValidationGroup="Input" ID="tbQty" Width="80px"/> </td>                
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbUnit" Width="80px"/> </td>                
                    </tr>  
                    
                    <tr>
                        <td>Production Date</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbProdDate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                        </td>      
                    </tr>
                    <tr>
                        <td>Machine</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbMachine" Width="120px" AutoPostBack="true" ValidationGroup="Input"/> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbMachineName" Width="250px"/>    
                            <asp:Button class="bitbtn btngo" runat="server" ID="btnMachine" Text="..." ValidationGroup="Input"/>
                            <asp:Label runat ="server" Text="*" ID="Label1" ForeColor="Red"/>
                        </td>
                    </tr> 
                    <tr>
                        <td>Charting Position #1</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbChartingPosition" Width="80px"/> </td>                
                        <td>Charting Position #2</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDesignPrinting" Width="80px" ValidationGroup="Input"/>                            
                        </td>
                        <%--<td>Auto Generate WO / Level</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFgAutoGenerateWO" Width="40px" ValidationGroup="Input"> 
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem Selected="True">N</asp:ListItem>                    
                            </asp:DropDownList> 
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlAutoGenerateLevel" Width="40px" ValidationGroup="Input"> 
                            <asp:ListItem Selected="True">0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>                    
                            <asp:ListItem>2</asp:ListItem>                    
                            </asp:DropDownList> 
                        </td>     --%>           
                        
                    </tr>
                   
                    <tr>
                        <td>DO Date</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbDODate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                        </td>      
                    </tr> 
                <tr>
                    <td>
                        Subkon
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlSubkon" runat="server" CssClass="DropDownList" Height="16px"
                            Enabled="true">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem Selected="True">N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                    <%--<tr>
                        <td>Design Code</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDesignCode" Width="350px" ValidationGroup="Input"/>                            
                        </td>
                    </tr>   --%> 
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" ValidationGroup="Input" Width="350px" MaxLength ="255"/>            
                        </td>
                    </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                              
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                              
            <br />
       </asp:Panel> 
       <asp:Panel runat="server" ID="pnlDt2" Visible="False">
       
            <%--<asp:Label ID="Label1" runat="server" Text="Product Finish Goods :" />
            <asp:Label ID="lbProductFGCode" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product Code" />
            <asp:Label ID="lbProductFGName" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product Name" />
            <br />
            
            <asp:Label ID="Label6" runat="server" Text="BOM No - Formula :" />
            <asp:Label ID="lbBOMNo" runat="server" Font-Bold="true" ForeColor="Blue" Text="BOM No" />
            <asp:Label ID="lbFormula" runat="server" Font-Bold="true" ForeColor="Blue" Text="Formula" />
            <br />--%>
            
            <asp:Label ID="Label7" runat="server" Text="Process :" />
            <asp:Label ID="lbProcessCode" runat="server" Font-Bold="true" ForeColor="Blue" Text="Process Code" />
            <asp:Label ID="lbProcessName" runat="server" Font-Bold="true" ForeColor="Blue" Text="Process name" />
            <br /><br />
        
          <%--<asp:Button class="bitbtn btnadd" runat="server" ID="Button1" Text="Add" ValidationGroup="Input" />	                  --%>
             <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btnback" Text="Back" 
                 Width="60" />
            <br /><br />
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                         </asp:TemplateField>
                         
                        <asp:BoundField DataField="Material" HeaderStyle-Width="80px" HeaderText="Material Code" />
                        <asp:BoundField DataField="Material_Name" HeaderStyle-Width="250px" HeaderText="Material Name" />                        
                        <asp:BoundField DataField="MaterialAlt" HeaderStyle-Width="80px" HeaderText="Material Alt Code" />
                        <asp:BoundField DataField="MaterialAlt_Name" HeaderStyle-Width="250px" HeaderText="Material Alt Name" />
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" HeaderText="Specification" />                        
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.0000}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign = "Right" HeaderStyle-Width="80px" HeaderText="Qty"/>                        
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit"/>
                        <asp:BoundField DataField="QtyWaste" DataFormatString="{0:#,##0.0000}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign = "Right" HeaderStyle-Width="80px" HeaderText="Qty Waste"/>
                        <asp:BoundField DataField="QtyTotal" DataFormatString="{0:#,##0.0000}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign = "Right" HeaderStyle-Width="80px" HeaderText="Qty Total"/>                        
                    </Columns>
            </asp:GridView>
          </div>
        <br />
       <%--<asp:Button class="bitbtn btnadd" runat="server" ID="Button2" Text="Add" ValidationGroup="Input" />	  --%>
           
             <asp:Button ID="btnBackDt2ke2" runat="server" class="bitbtndt btnback" Text="Back" Width="60" />
           
       </asp:Panel>
       <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
            <table>                   
                    <tr>
                        <td>Material</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbMaterialCode" Width="120px" Enabled="False" /> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbMaterialName" Width="250px"/>    
                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnMaterialAlt" Text="Change Material Alternate" Width="160px" ValidationGroup="Input"/>
                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnMaterial" Text="Material Alternate" Width="160px" ValidationGroup="Input"/>
                            <%--<asp:Label runat ="server" Text="*" ID="Label8" ForeColor="Red"/>--%>
                        </td>
                        
                    </tr> 
                    <tr>
                        <td>Material Alternate</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" runat="server" ID="tbMaterialAltCode" Width="120px" Enabled="False" /> 
                            <asp:TextBox CssClass="TextBox" Enabled="False" runat="server" ID="tbMaterialAltName" Width="250px"/>    
                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnResMaterialAlt" Text="Reset Material Alternate" Width="160px" ValidationGroup="Input"/>
                            <%--<asp:Label runat ="server" Text="*" ID="Label8" ForeColor="Red"/>--%>
                        </td> 
                    </tr>
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbSpecificationDt2" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" Enabled="False"  Width="350px" MaxLength ="255"/>            
                        </td>
                    </tr>
                                                            
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" AutoPostBack="true" ValidationGroup="Input" ID="tbQtyDt2" Width="80px"/> </td>                
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbUnitDt2" Width="80px"/> </td>                
                    </tr>  
                    <tr>
                        <td>Qty Waste</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbQtyWaste" Width="80px"/> </td>                
                        <td>Qty Total</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" Enabled="False" ID="tbQtyTotal" Width="80px"/></td>                                        
                    </tr>
                    </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save"/>                              
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel"/>                              
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

