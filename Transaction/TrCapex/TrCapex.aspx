<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCapex.aspx.vb" Inherits="Transaction_TrCapex_TrCapex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
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
    
        function setformatdt()
        {
         try
         {  
//            document.getElementById("tbTotal").value = parseInt(document.getElementById("tb01").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb02").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb03").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb04").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb05").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb06").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb07").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb08").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb09").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb10").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb11").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tb12").value.replace(/\$|\,/g,"")) + 
//                                                       parseInt(document.getElementById("tbTotal01").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal02").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal03").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal04").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal05").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal06").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal07").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal08").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal09").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal10").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal11").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbTotal12").value.replace(/\$|\,/g,"")) +
//                                                       parseInt(document.getElementById("tbPrice").value.replace(/\$|\,/g,""));

            document.getElementById("tb01").value = setdigit(document.getElementById("tb01").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb02").value = setdigit(document.getElementById("tb02").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb03").value = setdigit(document.getElementById("tb03").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb04").value = setdigit(document.getElementById("tb04").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb05").value = setdigit(document.getElementById("tb05").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb06").value = setdigit(document.getElementById("tb06").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb07").value = setdigit(document.getElementById("tb07").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb08").value = setdigit(document.getElementById("tb08").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb09").value = setdigit(document.getElementById("tb09").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb10").value = setdigit(document.getElementById("tb10").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb11").value = setdigit(document.getElementById("tb11").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tb12").value = setdigit(document.getElementById("tb12").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tbTotal01").value = setdigit(document.getElementById("tbTotal01").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal02").value = setdigit(document.getElementById("tbTotal02").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal03").value = setdigit(document.getElementById("tbTotal03").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal04").value = setdigit(document.getElementById("tbTotal04").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal05").value = setdigit(document.getElementById("tbTotal05").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal06").value = setdigit(document.getElementById("tbTotal06").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal07").value = setdigit(document.getElementById("tbTotal07").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal08").value = setdigit(document.getElementById("tbTotal08").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal09").value = setdigit(document.getElementById("tbTotal09").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal10").value = setdigit(document.getElementById("tbTotal10").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal11").value = setdigit(document.getElementById("tbTotal11").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotal12").value = setdigit(document.getElementById("tbTotal12").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            
        }catch (err){
            alert(err.description);
          }      
        }
    
        function setformatdt2()
        {
         try
         {  
            //document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
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
        <div class="H1">Capex</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True">Year</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>                      
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
                  <asp:ListItem Selected="True">Year</asp:ListItem>
                  <asp:ListItem>Revisi</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
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
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />	                               
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
                              <asp:ListItem Text="Revisi" />
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />	                               
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="Year" SortExpression="Year" HeaderText="Year"></asp:BoundField>
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" SortExpression="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	                               
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G" />	                               
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlYear" />
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor = "Red"></asp:Label>
            </td>           
            
            <td>Revisi</td>
            <td>:</td>
            <td><asp:Label runat="server" ID="lbRevisi"></asp:Label></td>
        </tr>              
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" 
                    ID="tbRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />
            
                &nbsp;<asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Data" />	                               
                    
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" 
                ValidationGroup="Input" />	                               
            <div style="border:0px  solid; overflow:auto;">
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
                        </asp:TemplateField>
                        
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbDepartment" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Department") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbCategory" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CapexCategory") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        
                        <asp:BoundField DataField="CapexCategory" HeaderStyle-Width="120px" HeaderText="Capex Category Code" >
                        <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CapexCategoryName" HeaderStyle-Width="180px" HeaderText="Capex Category Name" >
                        <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DeptName" HeaderText="Department" HeaderStyle-Width="200px" ><HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Price" HeaderStyle-Width="100px" HeaderText="Price" ><HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty01" HeaderStyle-Width="100px" HeaderText="January" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total01" HeaderStyle-Width="100px" HeaderText="Total 01" ><HeaderStyle Width="100px" /></asp:BoundField>                        
                        <asp:BoundField DataField="Qty02" HeaderStyle-Width="100px" HeaderText="February" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total02" HeaderStyle-Width="100px" HeaderText="Total 02" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty03" HeaderStyle-Width="100px" HeaderText="March" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total03" HeaderStyle-Width="100px" HeaderText="Total 03" ><HeaderStyle Width="100px" /></asp:BoundField>                        
                        <asp:BoundField DataField="Qty04" HeaderStyle-Width="100px" HeaderText="April" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total04" HeaderStyle-Width="100px" HeaderText="Total 04" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty05" HeaderStyle-Width="100px" HeaderText="May" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total05" HeaderStyle-Width="100px" HeaderText="Total 05" ><HeaderStyle Width="100px" /></asp:BoundField>                        
                        <asp:BoundField DataField="Qty06" HeaderStyle-Width="100px" HeaderText="June" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total06" HeaderStyle-Width="100px" HeaderText="Total 06" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty07" HeaderStyle-Width="100px" HeaderText="July" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total07" HeaderStyle-Width="100px" HeaderText="Total 07" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty08" HeaderStyle-Width="100px" HeaderText="August" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total08" HeaderStyle-Width="100px" HeaderText="Total 08" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty09" HeaderStyle-Width="100px" HeaderText="September" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total09" HeaderStyle-Width="100px" HeaderText="Total 09" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty10" HeaderStyle-Width="100px" HeaderText="October" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total10" HeaderStyle-Width="100px" HeaderText="Total 10" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty11" HeaderStyle-Width="100px" HeaderText="November" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total11" HeaderStyle-Width="100px" HeaderText="Total 11" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Qty12" HeaderStyle-Width="100px" HeaderText="December" ><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total12" HeaderStyle-Width="100px" HeaderText="Total 12" ><HeaderStyle Width="100px" /></asp:BoundField>                        
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="100px" HeaderText="Qty" Visible = "false"><HeaderStyle Width="100px" /></asp:BoundField>
                        <asp:BoundField DataField="Total" HeaderStyle-Width="100px" HeaderText="Total" Visible = "false"><HeaderStyle Width="100px" /></asp:BoundField>                        
                    </Columns>
                </asp:GridView>
          </div>  
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" 
                ValidationGroup="Input" />           
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" 
            Visible="false" Width="1148px">
            <table style="width: 1149px">              
                <tr>
                    <td>Capex Category</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbCategoryCode" runat="server" AutoPostBack="true" CssClass="TextBox" ValidationGroup="Input" Width="60px" />
                        <asp:TextBox ID="tbCategoryName" runat="server" CssClass="TextBoxR" Enabled = "false" Width="322px"/>
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnCategory" Text="..."/>	                                                                                                          
                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Department</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="200px" />
                        <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Price</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbPrice" runat="server" AutoPostBack="true" CssClass="TextBox" 
                            ValidationGroup="Input" Width="110px" />
                        <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Qty/Total</td>
                    <td>:</td>
                    <td>
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>January</td>
                                <td>February</td>
                                <td>March</td>
                                <td>April</td>
                                <td>May</td>
                                <td>June</td>                               
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb01" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal01" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb02" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal02" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb03" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal03" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb04" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal04" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb05" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal05" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb06" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal06" Width="99px" 
                                        AutoPostBack="True"/></td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
                <tr>
                    <td>&nbsp</td>
                    <td>&nbsp</td>
                    <td>
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>July</td>
                                <td>August</td>
                                <td>September</td>
                                <td>October</td>
                                <td>November</td>
                                <td>December</td>                               
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb07" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal07" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb08" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal08" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb09" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal09" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb10" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal10" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb11" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal11" Width="99px" 
                                        AutoPostBack="True"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tb12" Width="43px" 
                                        AutoPostBack="True"/>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbTotal12" Width="99px" 
                                        AutoPostBack="True"/></td>
                            </tr>
                        </table>                    
                    </td>
                </tr>   
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="tbQty" runat="server" Visible = "false" CssClass="TextBox" Width="110px" />
                        <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBox" Visible="false" Width="110px" />
                    </td>
                </tr>
            </table>
            <br />        
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>             
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>             
       </asp:Panel> 
       <br />
       <asp:Button class="bitbtndt btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px" />                                   
       <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input" />                                   
       <asp:Button class="bitbtndt btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />                                   
       <asp:Button class="bitbtn btngo" runat="server" ID="btnHome" Text="Home" 
            Width="46px"/>                                   
    </asp:Panel>
    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
