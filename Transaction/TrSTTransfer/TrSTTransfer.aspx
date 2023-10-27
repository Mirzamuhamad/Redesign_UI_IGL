<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTTransfer.aspx.vb" Inherits="Transaction_TrSTTransfer_TrSTTransfer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    <div class="H1">    <asp:Label runat ="server" ID="lbTrans">CHANGE GOOD</asp:Label>
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
                      <asp:ListItem Value="Wrhs_Area_Name">Warehouse Area</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Src_Name">warehouse Source</asp:ListItem>
                      <asp:ListItem Value="Subled_Src_Name">Subled Source Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Dest_Name">warehouse Destination</asp:ListItem>
                      <asp:ListItem Value="Subled_Dest_Name">Subled Destination Name</asp:ListItem>                                            
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
                      <asp:ListItem Value="Wrhs_Area_Name">Warehouse Area</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Src_Name">warehouse Source</asp:ListItem>
                      <asp:ListItem Value="Subled_Src_Name">Subled Source Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Dest_Name">warehouse Destination</asp:ListItem>
                      <asp:ListItem Value="Subled_Dest_Name">Subled Destination Name</asp:ListItem>                                            
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
              <td>
                  <asp:Label ID="lbMemo" runat="server" 
                      ValidationGroup="Input" >Memo</asp:Label>
              </td>
            <td>:</td>                  
  
              <td>
                      <asp:TextBox ID="tbMemo" runat="server" AutoPostBack="true" 
                          CssClass="TextBox" />
              </td>
         </tr>     
              <tr>
                  <td>
                      Refference</td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlReffType" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="94px">
                          <asp:ListItem Value=" ">Choose One</asp:ListItem>
                          <asp:ListItem>Adjust</asp:ListItem>
                      </asp:DropDownList>
                      <asp:TextBox ID="tbReffNo" runat="server" CssClass="TextBoxR" Enabled="False" 
                          Width="200px" />
                      <asp:Button Class="btngo" ID="btnReffNo" Text="..." runat="server" />                                      
                          
                 
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
                      <asp:Button Class="bitbtndt btngetitem" ID="btnGetLHP" Text="Get Data LHP" Width = "90" runat="server" Visible="false" />                                      
                      
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
                               	<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductSrc" HeaderStyle-Width="120px" 
                            HeaderText="Product Src Code" SortExpression="ProductSrc" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Src_Name" HeaderText="Product Src Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Src_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductDest" HeaderStyle-Width="120px" 
                            HeaderText="Product Dest Code" SortExpression="ProductDest" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Dest_Name" HeaderText="Product Dest Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Dest_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySrc" HeaderText="Qty Src" DataFormatString="{0:#,##0.##}"  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="QtySrc" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitSrc" HeaderText="Unit Src" SortExpression="UnitSrc" />                                                                        
                        <asp:BoundField DataField="QtyDest" HeaderText="Qty Dest" DataFormatString="{0:#,##0.##}"  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                            SortExpression="QtyDest" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitDest" HeaderText="Unit Dest" SortExpression="Unit Dest" />                                                                        
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
                        <tr>
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
                        </tr>       
                    
                        <tr>
                            <td>
                                Source
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
                                <asp:TextBox ID="tbLevelProduct" runat="server" CssClass="TextBox" 
                                    Visible="False" Width="80px" />
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                Qty Source</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbQtySrc" runat="server" AutoPostBack="true" 
                                CssClass="TextBox" Width="80px" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbUnitSrc" runat="server" CssClass="TextBox" Enabled="false" 
                                Width="75px" />
                            </td>
                            <td>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Destination
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbProdDestCode" runat="server" AutoPostBack="True" 
                                    CssClass="TextBox" Width="150px" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbProdDestName" runat="server" CssClass="TextBoxR" 
                                    Enabled="False" EnableTheming="True" ReadOnly="True" Width="200px" />
                            </td>
                            <td>
                                <asp:Button ID="btnProdDest" runat="server" Class="btngo" Text="..." />
                            </td>
                        </tr>                        
                        <tr>
                            <td >
                                Qty Destination
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDest" Width="80px" />                             
                            </td>
                            <td>    
                                <asp:TextBox ID="tbUnitDest" runat="server" Width="75px" CssClass="TextBox" 
                                Enabled="false" />
                            </td>                                
                            <td>
                            </td>
                        </tr>                     
                        <tr>
                            <td>
                                Remark</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox ID="tbRemarkDt" runat="server" AutoPostBack="False" 
                                    CssClass="TextBox" Height="41px" MaxLength="60" TextMode="MultiLine" 
                                    Width="354px" />
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
    
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
