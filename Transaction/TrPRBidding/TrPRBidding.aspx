<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPRBidding.aspx.vb" Inherits="Transaction_TrPRBidding_TrPRBidding" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Bidding</title>
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
        //var Rate = document.getElementById("tbRate");
        //var DrForex = document.getElementById("tbDebitForex");
        //var CrForex = document.getElementById("tbCreditForex");
        
         try
         {           
        //document.getElementById("tbOldPrice").value = setdigit(document.getElementById("tbOldPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        //document.getElementById("tbNewPrice").value = setdigit(document.getElementById("tbNewPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            width: 1px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Supplier Bidding</div>
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
                      <asp:ListItem Value="Type">Type</asp:ListItem>
                      <asp:ListItem Value="TopRecord">Top Records</asp:ListItem>
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Type">Type</asp:ListItem>
                      <asp:ListItem Value="TopRecord">Top Records</asp:ListItem>
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
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"><HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate"><HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Type" htmlencode="true" HeaderText="Type" SortExpression="Type" ><HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TopRecord" HeaderStyle-Width="80px" HeaderText="Top Records" SortExpression="TopRecord"><HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderText="Remark"><HeaderStyle Width="250px" />
                  </asp:BoundField>
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
            <td style="margin-left: 40px"><asp:TextBox CssClass="TextBox" runat="server" ID="tbRef" Width="149px" Enabled="False"/> &nbsp; &nbsp; 
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
            <td>Type</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="DropDownList" 
                    Width="110px" >
                    <asp:ListItem Selected="True">Automatic</asp:ListItem>
                    <asp:ListItem>Manual</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
          <tr>
              <td>Top Records</td>
              <td>:</td>
              <td><asp:TextBox ID="tbTop" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="47px" />
              </td>
          </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td><asp:TextBox ID="tbRemark" runat="server" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" ValidationGroup="Input" Width="269px" /> &nbsp;
                  <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" ValidationGroup="Input" Width="56px"/>               
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />                 
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
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>	
                            </ItemTemplate>
                            <%--<EditItemTemplate>
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
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <%--baru sampai sini --%>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product Code" ><HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px" HeaderText="Product Name" ><HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Supplier" HeaderStyle-Width="80px" HeaderText="Supplier Code" ><HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="250px" HeaderText="Supplier Name" ><HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" />
                        <asp:BoundField DataField="MOQ" HeaderText="MOQ" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="MOQ" />
                        <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Term" HeaderText="Top" ><HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DeliveryTime" HeaderStyle-Width="80px" HeaderText="Delivery Time (Days)" ><HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Selected" HeaderStyle-Width="80px" HeaderText="Selected" ><HeaderStyle Width="80px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                     
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/></td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbProduct" runat="server" CssClass="TextBox" Width="145px" AutoPostBack="True" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" Width="197px" Enabled="False" />
                        <asp:Button class="btngo" runat="server" ID="BtnProduct" Text="..." ValidationGroup="Input"/>     
                    </td>
                </tr>
                <tr>
                    <td>Supplier</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbSuppCode" runat="server" AutoPostBack="True" 
                            CssClass="TextBoxR" Width="145px" Enabled="False" />
                        <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBoxR" Enabled="False" Width="197px" />
                    </td>
                </tr>
                <tr>
                    <td>Unit / MOQ</td>
                    <td class="style1">:</td>
                    <td><asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="110px" AutoPostBack ="true" Enabled = "false"/>
                        /
                        <asp:TextBox ID="tbPrice0" runat="server" CssClass="TextBox" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>Price</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" Enabled = "false"/>
                    </td>
                </tr>
                <tr>
                    <td>Top</td>
                    <td class="style1">:</td>                    
                    <td>
                        <asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="True" CssClass="DropDownList" Height="16px" Width="154px" Enabled = "false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Delivery Time</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbLeadTime" runat="server" CssClass="TextBox" Width="48px" Enabled = "false"/>&nbsp;Days</td>
                </tr>
                <tr>
                    <td>Selected</td>
                    <td class="style1">:</td>
                    <td><asp:DropDownList ID="ddlSelect" runat="server" AutoPostBack="true" 
                            CssClass="DropDownList" Width="37px" Height="16px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem Selected="True">N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>
            <br />
       </asp:Panel> 
       <br /> 
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New" ValidationGroup="Input" Width="96px"/>
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input" Width="64px"/>
       <asp:Button class="bitbtn btncancel" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" Width="73px"/>
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="62px"/>      
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" eight="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
