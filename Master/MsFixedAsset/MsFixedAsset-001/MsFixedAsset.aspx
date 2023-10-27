<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFixedAsset.aspx.vb" Inherits="MsFixedAsset" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        
        function setformatdt()
        {
        try
         {         
        
         }catch (err){
            alert(err.description);
          }      
        }   
        
        function OpenTransaction(_form, _keyid, _code) {
            window.open("../../Transaction/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code,"List","scrollbars=yes,resizable=no,width=700,height=500");            
            return false;
        }
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            width: 63px;
        }
        .style4
        {
            width: 72px;
        }
        .style5
        {
            width: 86px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lbTitle" Text= "Fixed Asset File"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="FA_Code" Selected="True">Asset Code</asp:ListItem>
                      <asp:ListItem Value="FA_Name">Asset Name</asp:ListItem>
                      <asp:ListItem Value="FA_Sub_Group_Name">Asset SUb Group</asp:ListItem>
                      <asp:ListItem Value="FAGroup_Name">Asset Group</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(BuyingDate)">Buying Date</asp:ListItem>
                      <asp:ListItem Value="QtyBalance">Qty Balance</asp:ListItem>
                      <asp:ListItem Value="LifeCurrent">Life Balance</asp:ListItem>                      
                      <asp:ListItem Value="Total_Current">Total Balance</asp:ListItem>                      
                      <asp:ListItem Value="Fg_Moving">Moving</asp:ListItem>
                      <asp:ListItem Value="FgActive">Active</asp:ListItem>                     
                  </asp:DropDownList>
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
                      <asp:ListItem Value="FA_Code" Selected="True">Asset Code</asp:ListItem>
                      <asp:ListItem Value="FA_Name">Asset Name</asp:ListItem>
                      <asp:ListItem Value="FA_Sub_Group_Name">Asset SUb Group</asp:ListItem>
                      <asp:ListItem Value="FAGroup_Name">Asset Group</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(BuyingDate)">Buying Date</asp:ListItem>
                      <asp:ListItem Value="QtyBalance">Qty Balance</asp:ListItem>
                      <asp:ListItem Value="LifeCurrent">Life Balance</asp:ListItem>                      
                      <asp:ListItem Value="Total_Current">Total Balance</asp:ListItem>
                                            <asp:ListItem Value="Fg_Moving">Moving</asp:ListItem>
                      <asp:ListItem Value="FgActive">Active</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>  
      <br/>        
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <%--<asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>--%>
                  <asp:TemplateField HeaderStyle-Width="110px" ItemStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />                              
                              <%--<asp:ListItem Text="Print" />--%>
                              <asp:ListItem Text="Photo" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>
                          </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="FA_Code" HeaderStyle-Width="100px" SortExpression="FA_Code" HeaderText="Asset Code" ItemStyle-Wrap="false"></asp:BoundField>                  
                  <asp:BoundField DataField="FA_Name" HeaderStyle-Width="200px" SortExpression="FA_Name" HeaderText="Asset Name" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="Specification" HeaderStyle-Width="300px" SortExpression="Specification" HeaderText="Specification" ItemStyle-Wrap="true"></asp:BoundField>                            
                  <asp:BoundField DataField="FA_Sub_Group_Name" HeaderStyle-Width="100px" SortExpression="FA_Sub_Group_Name" HeaderText="Asset Sub Group" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="FAGroup_Name" HeaderStyle-Width="100px" SortExpression="FAGroup_Name" HeaderText="Asset Group" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="BuyingDate" DataFormatString="{0:dd MMM yyyy}"  htmlencode="true" SortExpression="BuyingDate" HeaderText="Buying Date" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="QtyBalance" HeaderStyle-Width="80px" HeaderText="Qty Balance" SortExpression="QtyBalance" ItemStyle-HorizontalAlign = "Right" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="LifeCurrent" HeaderStyle-Width="80px" HeaderText="Life Balance" SortExpression="LifeCurrent" ItemStyle-HorizontalAlign = "Right" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="Total_Current" HeaderStyle-Width="80px" HeaderText="Total Balance(Rp)" SortExpression="Total_Current" ItemStyle-HorizontalAlign = "Right" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="Fg_Moving" HeaderText="Moving" SortExpression="Fg_Moving" ItemStyle-Wrap="false"></asp:BoundField>
                  <asp:BoundField DataField="FgActive" HeaderText="Active" SortExpression="FgActive" ItemStyle-Wrap="false"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td class="style5">Asset</td>
            <td>:</td>
            <td colspan="9"><asp:TextBox CssClass="TextBox" runat="server" ID="tbCode" Width="150px" Enabled="False"/>                         
                <asp:TextBox runat="server" ID="tbName" CssClass="TextBox" Width="326px" Enabled="False"/>
            </td>            
        </tr>                
        <tr>
            <td class="style5">Specification</td>
            <td>:</td>
            <td colspan="9">
                <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBox" Enabled="False" TextMode="MultiLine" Width="326px" />
            </td>
        </tr>                                                                
        <tr>
            <td class="style5">Asset Sub Group</td>
            <td>:</td>
            <td colspan="9"><asp:DropDownList CssClass="DropDownList" ID="ddlFASubGroup" 
                    runat="server" Enabled="False"></asp:DropDownList> 
            </td>    
        </tr>
        <tr>
            <td class="style5">Asset Group</td>
            <td>:</td>
            <td colspan="9"><asp:DropDownList CssClass="DropDownList" ID="ddlFAGroup" 
                    runat="server" Enabled="False"></asp:DropDownList> 
            </td>    
        </tr>
        <tr>
            <td class="style5">Asset Status</td>
            <td>:</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlFAStatus" runat="server" CssClass="DropDownList" 
                    Enabled="False">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td colspan="2" align="right">Unit :</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Enabled="False">
                </asp:DropDownList>
            </td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
          <tr>
              <td class="style5">
                  Buying Date</td>
              <td>
                  :</td>
              <td colspan="2">
                  <BDP:BasicDatePicker ID="tbBuyingDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBox" 
                      Enabled="False" ReadOnly="true" ShowNoneButton="False" 
                      TextBoxStyle-CssClass="TextDate">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>
              <td>
                  &nbsp;</td>
              <td colspan="2" align="right">
                  Cost Center :</td>
              <td colspan="2">
                  <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                      Enabled="False">
                  </asp:DropDownList>
              </td>
              <td colspan="2">
                  &nbsp;</td>
          </tr>
        <tr>
            <td class="style5">Qty</td>
            <td>:</td>
            <td colspan="9">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>
                            Buying</td>
                        <td>
                            Opname</td>
                        <td>
                            Sold</td>
                        <td>
                            Balance</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbQtyBuying" runat="server" CssClass="TextBox" 
                                Enabled="False" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbQtyOpname" runat="server" CssClass="TextBox" 
                                Enabled="False" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbQtySold" runat="server" CssClass="TextBox" Enabled="False" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbQtyBalance" runat="server" CssClass="TextBox" 
                                Enabled="False" />
                        </td>
                    </tr>
                </table>
            </td>            
        </tr>
        <tr>
            <td class="style5">&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="9" rowspan="3">
                <table>
                <tr style="background-color:Silver;text-align:center">
                     <td>Buying</td>
                     <td>Begin Depr</td>
                     <td>Processed Depr</td>
                     <td>Total Depr</td>
                     <td>Balance</td>
                     <!--<td>NDA</td>-->
                </tr>
                <tr>
                     <td><asp:TextBox runat="server" ID="tbLifeBuying" CssClass="TextBox" Enabled="False"/></td>
                     <td><asp:TextBox runat="server" ID="tbBeginDepr" CssClass="TextBox"   Enabled="False"/></td>
                     <td><asp:TextBox runat="server" ID="tbProcessedDepr" CssClass="TextBox"   Enabled="False"/></td>
                     <td><asp:TextBox runat="server" ID="tbTotalLiveDepr" CssClass="TextBox"   Enabled="False"/></td>
                     <td><asp:TextBox runat="server" ID="tbLifeBalance" CssClass="TextBox" Enabled="False"/></td>
                     <td></td>                     
                </tr>
                    <tr>
                      <td><asp:TextBox ID="tbTotalBuying" runat="server" CssClass="TextBox" Enabled="False" /></td>
                      <td><asp:TextBox ID="tbBeginTotalDepr" runat="server" CssClass="TextBox" Enabled="False" /></td>
                      <td><asp:TextBox ID="tbTotalProcessDepr" runat="server" CssClass="TextBox" Enabled="False" /></td>
                      <td><asp:TextBox ID="tbTotalDepr" runat="server" CssClass="TextBox" Enabled="False" /></td>
                      <td><asp:TextBox ID="tbTotalBalance" runat="server" CssClass="TextBox" Enabled="False" /></td>
                      <td></td>
                      <!--<asp:TextBox ID="tbTotalNDA" runat="server" CssClass="TextBox" Enabled="False" />-->
                    </tr>
                </table>
             </td>
        </tr> 
          <tr>
              <td class="style5">Life</td>
              <td>:</td>
          </tr>
        <tr>
            <td class="style5">Value</td>
            <td>:</td>
        </tr> 
        <tr>
           <td class="style5">Expendable</td>
            <td>:</td>
            <td class="style4"><asp:TextBox runat="server" ID="tbFgExpendable" Width="20px" 
                    CssClass="TextBox" Enabled="False"/></td>
            <td style="text-align: right" colspan="3">Moving :</td>
            <td class="style2" colspan="2">
                <asp:TextBox ID="tbFgMoving" runat="server" CssClass="TextBox" Enabled="False" 
                    Width="20px" />
            </td>
            <td colspan="2">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>               
          <tr>
              <td class="style5">
                  Process</td>
              <td>
                  :</td>
              <td class="style4">
                  <asp:TextBox ID="tbFgProcess" runat="server" CssClass="TextBox" Enabled="False" 
                      Width="20px" />
              </td>
              <td colspan="3" style="text-align: right">
                  Active :</td>
              <td class="style2" colspan="2">
                  <asp:TextBox ID="tbFgActive" runat="server" CssClass="TextBox" Enabled="False" 
                      Width="20px" />
              </td>
              <td colspan="2">
              </td>
              <td>
              </td>
          </tr>
      </table>  
      
      <br />  
      <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail Location" Value="0"></asp:MenuItem>
            </Items>
        </asp:Menu>
        
        
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="Tab1" runat="server">
         <br />      
          <asp:Panel runat="server" ID="pnlDt">
           <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"
                    ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" Wrap="false" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <%--<asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server"  
                                    ImageUrl="../../Image/btnEditDtOn.png"
                                    onmouseover="this.src='../../Image/btnEditDtOff.png';"
                                    onmouseout="this.src='../../Image/btnEditDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Edit" />   
                                <asp:ImageButton ID="btnDelete" runat="server" Visible="false" 
                                    ImageUrl="../../Image/btnDeleteDtOn.png"
                                    onmouseover="this.src='../../Image/btnDeleteDtOff.png';"
                                    onmouseout="this.src='../../Image/btnDeleteDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Delete" />     
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
                            
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="FALocationType" HeaderStyle-Width="80px" HeaderText="Location Type" SortExpression = "FALocationType"><HeaderStyle Width="80px" /></asp:BoundField>
                        <asp:BoundField DataField="FALocationCode" HeaderStyle-Width="80px" HeaderText="Location Code" SortExpression = "FALocationCode"><HeaderStyle Width="80px" /></asp:BoundField>
                        <asp:BoundField DataField="FALocationName" HeaderStyle-Width="250px" HeaderText="Location Name" SortExpression = "FALocationName"><HeaderStyle Width="250px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" SortExpression = "Qty"><HeaderStyle Width="80px" /></asp:BoundField>
                     </Columns>
                </asp:GridView>
          </div>   
        </asp:Panel>
        </asp:View>
       </asp:MultiView>  
       
       <br />   
       <asp:Button class="bitbtndt btnback" runat="server" ID="btnHome" Text="Home"/>                        
      </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
