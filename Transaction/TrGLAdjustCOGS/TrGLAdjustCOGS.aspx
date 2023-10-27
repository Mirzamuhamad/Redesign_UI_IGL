<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLAdjustCOGS.aspx.vb" Inherits="Transaction_TrGLAdjustCOGS_TrGLAdjustCOGS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title
    >
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">   
        function createajax()  
        { 
            var xmlhttp;
            if (window.XMLHttpRequest)
            {// code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp=new XMLHttpRequest();
            }
            else
            {// code for IE6, IE5
                xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
            }      
        }
        
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
    <div class="H1">Adjust Process COGS</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>
                      <asp:ListItem Value="WarehouseName">Warehouse Name</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>                                            
                      <asp:ListItem>Remark</asp:ListItem>
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
                  <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>
                  <asp:ListItem Value="WarehouseName">Warehouse Name</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>                                            
                  <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
            
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="100px" SortExpression="TransNmbr" HeaderText="Trans. No"></asp:BoundField>
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="WarehouseName" HeaderStyle-Width="100px" SortExpression="WarehouseName" HeaderText="Warehouse"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	 
                        
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Trans No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbRef" Width="149px"/> &nbsp; &nbsp; 
                <asp:Button class="bitbtn btnsearch" runat="server" ID="BtnGetDt" Text="Get Item" ValidationGroup="Input"/>
            </td>            
        </tr>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>                
            </td>            
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>      
        <tr>
            <td>Warehouse</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true" Height="16px" Width="230px">
                </asp:DropDownList>
                <asp:Label ID="Label7" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                          
            </td>
            <td>
                Effective Date</td>
            <td>
                :</td>
            <td>
                <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" Enabled="False" ReadOnly="true" 
                    ShowNoneButton="false" TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>
        </tr>
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" Width="300px" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" TextMode="MultiLine" /></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
           <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	 
             <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" AllowPaging="true" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />                    
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server"  
                                    ImageUrl="../../Image/btnUpdateDtOn.png"
                                    onmouseover="this.src='../../Image/btnUpdateDtOff.png';"
                                    onmouseout="this.src='../../Image/btnUpdateDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Update" />   
                                <asp:ImageButton ID="btnCancel" runat="server"  
                                    ImageUrl="../../Image/btnCancelDtOn.png"
                                    onmouseover="this.src='../../Image/btnCancelDtOff.png';"
                                    onmouseout="this.src='../../Image/btnCancelDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Cancel" />    
                            </EditItemTemplate>
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" HeaderText="Product" />
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px"  HeaderText="Product Name" />
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="250px"  HeaderText="Specification" />
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" />                        
                        <asp:BoundField DataField="OldPrice" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Current Price" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="NewPrice" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="New Price" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Total" DataFormatString="{0:#,##0.00}"  HeaderStyle-Width="80px" HeaderText="Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	 
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProduct" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbProductName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="267px"/> 
                        <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..." ValidationGroup="Input"/>            
                    </td>           
                </tr>                                    
                
                
                <tr>
                    <td>Specifcfication</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" 
                            Enabled="False" EnableTheming="True" ReadOnly="True" Width="409px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="80px" />
                        <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Price COGS</td>
                    <td>
                        :</td>
                    <td>
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Current</td>
                                <td>
                                    New</td>
                                <td>
                                    Total Adjust</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbCurrent" runat="server" CssClass="TextBoxR" Enabled="False" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbNew" runat="server" CssClass="TextBox" Enabled="True" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" 
                            TextMode="MultiLine" Width="300px" />
                    </td>
                </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/>    
    </asp:Panel>
    
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
