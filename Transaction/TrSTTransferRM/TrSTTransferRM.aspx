<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTTransferRM.aspx.vb" Inherits="Transaction_TrSTTransferRM_TrTransferRM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Transfer</title>
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
        //document.getElementById("tbQtyOrder").value = setdigit(document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtySrc").value = setdigit(document.getElementById("tbQtySrc").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyDest").value = setdigit(document.getElementById("tbQtyDest").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');        
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
    <div class="H1">    <asp:Label runat ="server" ID="lbTrans">Transfer Material Or 
        Retur Or Project</asp:Label>
        </div>
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
                      <asp:ListItem Value="ShiftName" >Shift</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr" >Reff No</asp:ListItem>
                      <asp:ListItem Value="ProductFG" >Product FG</asp:ListItem>
                      <asp:ListItem Value="ProductFG_Name" >Product FGName</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Area_Name">Warehouse Area</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Src_Name">warehouse Source</asp:ListItem>
                      <asp:ListItem Value="Subled_Src_Name">Subled Source Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Dest_Name">warehouse Destination</asp:ListItem>
                      <asp:ListItem Value="Subled_Dest_Name">Subled Destination Name</asp:ListItem>                                            
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver">Driver</asp:ListItem>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="ShiftName" >Shift</asp:ListItem>
                      <asp:ListItem Value="ReffNmbr" >Reff No</asp:ListItem>
                      <asp:ListItem Value="ProductFG" >Product FG</asp:ListItem>
                      <asp:ListItem Value="ProductFG_Name" >Product FGName</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Area_Name">Warehouse Area</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Src_Name">warehouse Source</asp:ListItem>
                      <asp:ListItem Value="Subled_Src_Name">Subled Source Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Dest_Name">warehouse Destination</asp:ListItem>
                      <asp:ListItem Value="Subled_Dest_Name">Subled Destination Name</asp:ListItem>                                            
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver">Driver</asp:ListItem>
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
                              <asp:ListItem Text="Input Lot No" />                              
                              <asp:ListItem Text="Print" />
                              <asp:ListItem Text="Print Sample" />
                          </asp:DropDownList>
                         <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                    
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                      HeaderText="Date" SortExpression="TransDate">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ShiftName" HeaderText="Shift" 
                      SortExpression="ShiftName" > <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ReffNmbr" HeaderText="Reff No" 
                      SortExpression="ReffNmbr" >
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  
                  <asp:BoundField DataField="Wrhs_Area_Name" HeaderText="Warehouse Area" 
                      SortExpression="Wrhs_Area_Name" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Wrhs_Src_Name" 
                      HeaderText="Warehouse Source" SortExpression="Wrhs_Src_Name">
                  </asp:BoundField>
                  <asp:BoundField DataField="Subled_Src_Name" HeaderText="Subled Source" 
                      SortExpression="Subled_Src_Name" />
                  <asp:BoundField DataField="Wrhs_Dest_Name" HeaderText="Warehouse Destination" 
                      SortExpression="Wrhs_Dest_Name" />
                  <asp:BoundField DataField="Subled_Dest_Name" HeaderText="Subled Destination" 
                      SortExpression="Subled_Dest_Name" />                      
                  <asp:BoundField DataField="Operator" HeaderText="Operator" 
                      SortExpression="Operator" />                                            
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbTransNo" Width="149px"/> 
            </td>            
        </tr>
        <tr>
            <td>Date / Shift</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:DropDownList ID="ddlShift" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input" />
            </td>                        
        </tr>    
              <tr>
                  <td>
                      Reff No</td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlReffType" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="94px">
                          <asp:ListItem>Material</asp:ListItem>
                          <asp:ListItem>Return</asp:ListItem>
                          <asp:ListItem>Project</asp:ListItem>
                      </asp:DropDownList>
                      <asp:TextBox ID="tbReffNo" runat="server" CssClass="TextBoxR" Enabled="False" 
                          Width="200px" />
                      <asp:Button Class="btngo" ID="btnReffNo" Text="..." runat="server" />                                      
                          
                 
                      <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
                          
                 
                  </td>
                  <td>
                      <asp:Label ID="lbRM" runat="server">Product FG</asp:Label>
                  </td>
                  <td>
                      <asp:Label ID="lbTitik" runat="server">:</asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="tbCodeFG" runat="server" CssClass="TextBoxR" Enabled="False" 
                          Width="76px" />
                      <asp:TextBox ID="tbNameFG" runat="server" CssClass="TextBoxR" Enabled="False" 
                          Width="200px" />
                  </td>
              </tr>
              <tr>
                  <td>
                      Warehouse Area</td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlWrhsArea" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                      </asp:DropDownList>
                      <asp:Label ID="lbred2" runat="server" ForeColor="Red">*</asp:Label>
                  </td>
          </tr>
              <tr>
                  <td>
                      Warehouse Source</td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlWrhsSrc" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                      </asp:DropDownList>
                      <asp:TextBox ID="tbFgSubledSrc" runat="server" AutoPostBack="true" 
                          CssClass="TextBox" Visible="False" />
                      <asp:TextBox ID="tbType" runat="server" AutoPostBack="true" CssClass="TextBox" 
                          Visible="False" />
                  </td>
              </tr>
              <tr>
                  <td>
                      Subled Source</td>
                  <td>
                      :</td>
                  <td>
                      <asp:TextBox ID="tbSubledSrc" runat="server" AutoPostBack="True" 
                          CssClass="TextBox" />
                      <asp:TextBox ID="tbSubledSrcName" runat="server" CssClass="TextBoxR" 
                          Enabled="False" Width="200px" />
                     
                      <asp:Button Class="btngo" ID="btnSubledSrc" Text="..." runat="server" />                                      
                      
                      
                  </td>
              </tr>
              <tr>
                  <td>
                      Warehouse Destination</td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlWrhsDest" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                      </asp:DropDownList>
                      <asp:TextBox ID="tbFgSubledDest" runat="server" AutoPostBack="true" 
                          CssClass="TextBox" Visible="False" />
                      <asp:Label ID="lbred3" runat="server" ForeColor="Red">*</asp:Label>
                  </td>
              </tr>
              <tr>
                  <td>
                      Subled Destination</td>
                  <td>
                      :</td>
                  <td>
                      <asp:TextBox ID="tbSubledDest" runat="server" AutoPostBack="True" 
                          CssClass="TextBox" />
                      <asp:TextBox ID="tbSubledDestName" runat="server" CssClass="TextBoxR" 
                          Enabled="False" Width="200px" />
                      <asp:Button Class="btngo" ID="BtnSubledDest" Text="..." runat="server" />                                      
                  </td>
              </tr>
              <tr>
                  <td>
                      Operator</td>
                  <td>
                      :</td>
                  <td>
                      <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" 
                          ValidationGroup="Input" Width="236px" />
                      <asp:Button Class="bitbtndt btngetitem" ID="btnGetDt" Text="Get Data" Width = "70" runat="server" ValidationGroup="Input" />                                      
                      
                  </td>
              </tr>
              <tr>
                  <td>
                      Car No / Driver</td>
                  <td>
                      :</td>
                  <td>
                      <asp:TextBox ID="tbCarNo" runat="server" CssClass="TextBox" 
                          ValidationGroup="Input" Width="120px" />
                      <asp:TextBox ID="tbDriver" runat="server" CssClass="TextBox" 
                          ValidationGroup="Input" Width="236px" />    
                  </td>
              </tr>
              
              <tr>
                  <td>
                      Remark</td>
                  <td>
                      :</td>
                  <td>
                      <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                          ValidationGroup="Input" Width="269px" />
                      &nbsp;&nbsp;&nbsp;&nbsp;
                      </td>
              </tr>
          </tr>        
      </table>  
      
        <asp:Panel ID="PnlInfo0" runat="server" BorderColor="Black" BorderStyle="Solid" Visible="false"
            Height="100%" Width="700px">
            &nbsp; <%--</asp:Panel></td>--%>
            <%-- <td class="style1">
                  <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
            Height="100%" Width="252px">--%>
            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="Set All Location Source"></asp:Label>
            &nbsp;
            <td>
                <asp:DropDownList ID="ddlLocationAllSrc" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList" Height="16px" Width="154px" />
                &nbsp;<asp:Button ID="btnProcess" runat="server" Class="bitbtndt btngo" 
                    Text="Process" ValidationGroup="Input" Width="70" />
                &nbsp;<br />
            </td>
            <%-- </asp:Panel></td>--%>
            <%--<td class="style1">
                  <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Solid" 
                      Height="100%" Width="83px">--%>
            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="Delete Selected Item"></asp:Label>
            &nbsp;&nbsp;<asp:Button ID="btnProcessDel" runat="server" Class="bitbtndt btngo" 
                Text="Process" ValidationGroup="Input" Width="70" />
            <br />
        </asp:Panel>
      
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
                    <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="cbSelectHd_CheckedChanged1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                               	<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductSrc" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" SortExpression="ProductSrc" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Src_Name" HeaderText="Product Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Src_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbLocationSrc" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "LocationSrc") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:BoundField DataField="Location_Src_Name" HeaderText="Location Src" HeaderStyle-Width="200px" 
                            SortExpression="Location_Src_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>   
                         <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbLocationDest" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "LocationDest") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:BoundField DataField="Location_Dest_Name" HeaderText="Location Dest" HeaderStyle-Width="200px" 
                            SortExpression="Location_Dest_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>  
                         <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbProductDest" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ProductDest") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                     
                        <%--<asp:BoundField DataField="ProductDest" HeaderStyle-Width="120px" 
                            HeaderText="Product Dest Code" SortExpression="ProductDest" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Dest_Name" HeaderText="Product Dest Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Dest_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="CostCtrSrcName" HeaderText="Cost Center" SortExpression="CostCtrName"></asp:BoundField>
                                               
                        <asp:BoundField DataField="QtySrc" DataFormatString="{0:#,##0.####}" HeaderText="Qty" SortExpression="QtySrc" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitSrc" HeaderText="Unit" SortExpression="UnitSrc" />                                                
                        <asp:BoundField DataField="QtyRollSrc" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Qty 2" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPackSrc" HeaderStyle-Width="80px" HeaderText="Unit 2" >                                                                        
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyM2Src" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Qty 3" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitM2Src" HeaderStyle-Width="80px" HeaderText="Unit 3" >                                                                        
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                       
                       <%-- <asp:BoundField DataField="QtyDest" DataFormatString="{0:#,##0.####}" HeaderText="Qty Dest" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                            SortExpression="QtyDest" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitDest" HeaderText="Unit Dest" SortExpression="Unit Dest" />                                                                        
                        <asp:BoundField DataField="QtyRollDest" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Qty Dest 2" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPackDest" HeaderStyle-Width="80px" HeaderText="Unit Dest 2" >                                                                        
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyM2Dest" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Qty Dest 3" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitM2Dest" HeaderStyle-Width="80px" HeaderText="Unit Dest 3" >                                                                        
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CostCtrDestName" HeaderText="Cost Center" SortExpression="CostCtrName"></asp:BoundField>
                        --%>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" 
                            HeaderText="Remark" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
        <table width="100%">
            <tr>
                <td style="Width:80%">            
                    <table>
                        <%--<tr>
                            <td>                        
                            </td>
                            <td>
                            </td>    
                            <td>
                                <center>
                                    <u>Product Code</u>                                                       
                                </center> 
                            </td>                        
                            <td>
                                <center>
                                    <u>Product Name</u>                                                       
                                </center> 
                            </td>    
                            <td></td>
                        </tr>--%>       
                    
                        <tr>
                            <td>
                                Product
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbProdSrcCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="150px" />
                            </td>
                            <td>        
                                <asp:TextBox ID="tbProdSrcName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="200px" />
                            </td>
                            <td>  
                            
                                <asp:Button Class="btngo" ID="btnProdSrc" Text= "..." runat="server" />                                                        
                                
                            </td>
                        </tr> 
                        
                        <tr>
                            <td>
                                Location Src/ Cost Center</td>
                            <td>:</td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlLocationSrc" runat="server" AutoPostBack="True" 
                                    CssClass="DropDownList" Width="150px" />
                                /<asp:DropDownList ID="ddlCostCtrSrc" runat="server" CssClass="DropDownList" 
                                    Width="150px" AutoPostBack="True" />
                            </td> 
                            <td> 
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                Location Dest</td>
                            <td>
                                :</td>
                            <td>
                                <asp:DropDownList ID="ddlLocationDest" runat="server" AutoPostBack="True" 
                                    CssClass="DropDownList" Width="150px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCostCtrDest" runat="server" CssClass="DropDownList" 
                                    Width="150px" />
                                <asp:Label ID="lbQtyPackDest" runat="server" Visible="False" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>      
                        <tr>
                            <td>
                                Qty </td>
                            <td>:</td>
                            <td colspan="2">
                                <table cellspacing="0" cellpadding="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>Qty
                                            <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                        <td>Unit 
                                            <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbQtyPackSrc" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbQtyM2Src" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtySrc" 
                                                AutoPostBack="true" Width="80px" /></td>
                                        <td>
                                            <asp:TextBox ID="tbUnitSrc" runat="server" Width="75px" CssClass="TextBox" 
                                                Enabled="false" />
                                        </td>
                                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRollSrc" Width="80px"/></td>
                                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyM2Src" Width="80px"/></td>        
                                    </tr>
                                </table></td> 
                            <td> </td>
                        </tr>                       
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="tbProdDestCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="150px" enabled ="false" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbProdDestName" runat="server" CssClass="TextBoxR" 
                                    Enabled="False" EnableTheming="True" ReadOnly="True" Width="200px" />
                            </td>
                            <td>
                                <asp:Button ID="btnProdDest" runat="server" Class="btngo" Text="..." enabled ="false" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td></td>
                            <td></td>
                            <td colspan="2">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbQtyDest" runat="server" AutoPostBack="true" 
                                                CssClass="TextBoxR" enabled="false" Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbUnitDest" runat="server" CssClass="TextBox" Enabled="false" 
                                                Width="75px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyRollDest" runat="server" CssClass="TextBox" 
                                                Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyM2Dest" runat="server" CssClass="TextBox" Width="80px" />
                                            <asp:Label ID="lbQtyM2Dest" runat="server" Visible="False"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td >
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="tbRemarkDt" runat="server" AutoPostBack="False" 
                                    CssClass="TextBox" Width="250px" TextMode="MultiLine" Height="26px" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
				<asp:Panel runat="server" ID="PnlInfo" Visible="false" Height="100%" Width="100%">
                    <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Stock :"></asp:Label>
                    <br />
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="false" ShowFooter="true">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:BoundField DataField="Code" HeaderStyle-Width="120px" HeaderText="Location" />
                                <asp:BoundField DataField="Qty" HeaderStyle-Width="70px" HeaderText="Qty" />
                                <asp:BoundField DataField="QtyMin" HeaderStyle-Width="70px" HeaderText="Qty Min" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>                    
                </td>
             </tr>
        </table>             
            
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
            <br />
       </asp:Panel> 
       <br />          
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <%--<cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />--%>
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
