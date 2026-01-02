<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPRReq.Aspx.vb" Inherits="TrPRReq" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Request Order</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
      <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript">

        function ProgressCircle() {
            setTimeout(function() {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function() {
            ProgressCircle();
        });
        
        function opencekpostingdlg() {
            window.open("../../Transaction/TrPRReq/FormCekPostPR.Aspx", "List", "scrollbars=yes,resizable=no,width=800,height=400");
            return false;

        }

        function setdigit(nStr, digit) {
            try {
                var TNstr = parseFloat(nStr);
                if (parseFloat(digit) >= 0) {
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
            } catch (err) {
                alert(err.description);
            }
        }

        function setformat() {
            var Qty = document.getElementById("tbQtyreq").value.replace(/\$|\,/g, "");
            var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g, "");
            //             var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
            //             var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");                
            try {
                document.getElementById("tbQtyreq").value = setdigit(Qty, '<%=ViewState("DigitQty")%>');
                document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs, '<%=ViewState("DigitQty")%>');
                // document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
                // document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
            } catch (err) {
                alert(err.description);
            }
        }

        function setformatDt2() {
            var Qty = document.getElementById("tbQtyOrder").value.replace(/\$|\,/g, "");

            try {
                document.getElementById("tbQtyOrder").value = setdigit(Qty, '<%=ViewState("DigitQty")%>');
            } catch (err) {
                alert(err.description);
            }
        }
        function closing() {
            try {
                var result = prompt("Remark Close", "");
                if (result) {
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            } catch (err) {
                alert(err.description);
            }
        }
        function deletetrans() {
            try {

                var result = confirm("Sure Delete Transaction ?");
                if (result) {
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
                }
                postback();
                //document.form1.submit();                
            } catch (err) {
                alert(err.description);
            }
        }
        function postback() {
            __doPostBack('', '');
        }   
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="TransNmbr">Request No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Request Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="DeptName">Department</asp:ListItem>
                      <asp:ListItem Value="RequestBy">Request By</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="UserPrep">User Prepare</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DatePrep)">Date Prepare</asp:ListItem>
                      <asp:ListItem Value="UserAppr">User Approval</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DateAppr)">Date Approval</asp:ListItem>
                      <asp:ListItem Value="Product">Product</asp:ListItem>                      
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>                      
                      
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
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="DeptName">Department</asp:ListItem>
                      <asp:ListItem Value="RequestBy">Request By</asp:ListItem>                                         
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="UserPrep">User Prepare</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DatePrep)">Date Prepare</asp:ListItem>
                      <asp:ListItem Value="UserAppr">User Approval</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DateAppr)">Date Approval</asp:ListItem>
                      <asp:ListItem Value="Product">Product</asp:ListItem>                      
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>                      
                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
            &nbsp;   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
          <br />&nbsp; 
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
         <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Request No">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="200px" SortExpression="ReffNmbr" HeaderText="Refference No.">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  
                  
                  <asp:BoundField DataField="DeptName" HeaderStyle-Width="200px" 
                      SortExpression="DeptName" HeaderText="Department">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="RequestBy" HeaderStyle-Width="120px" 
                      SortExpression="RequestBy" HeaderText="Request By">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="SVONo" HeaderStyle-Width="200px" SortExpression="ReffNmbr" HeaderText="SVO No.">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                      HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="UserPrep" HeaderStyle-Width="100px" 
                      SortExpression="UserPrep" HeaderText="User Prepare">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="DatePrep" HeaderStyle-Width="80px" 
                      SortExpression="DatePrep" HeaderText="Date Prepare" 
                      dataformatstring="{0:dd MMM yyyy}" htmlencode="true">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="UserAppr" HeaderStyle-Width="100px" 
                      HeaderText="User Approval" SortExpression="UserAppr">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="DateAppr" HeaderStyle-Width="80px" 
                      SortExpression="DateAppr" HeaderText="Date Approval" 
                      dataformatstring="{0:dd MMM yyyy}" htmlencode="true">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  
              </Columns>
          </asp:GridView>	    
          </div>
          <br />
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            
            &nbsp; 
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>  
      
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
        <tr>
            <td>Request No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>                    
            <td>Request Date</td>
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
            <td>Department</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlDept" CssClass="DropDownList" Width="200px"  />
                <asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>    &nbsp;</td>                      
        </tr>    
        
                                                    
        <tr>
            <td>Request By</td>
            <td>:</td>
            <td>    
                <asp:TextBox ID="tbRequestBy" runat="server" CssClass="TextBox" Enabled="False" 
                    EnableTheming="True" ReadOnly="True" Width="260px" />
            </td>   
            <td>
                <asp:Button ID="btnRequest" runat="server" Class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>
            <td>&nbsp;</td>
            <td>    
                <asp:Button ID="btnGetData" runat="server" class="bitbtn btnsearch" 
                    Text="Get Product" ValidationGroup="Input" Visible="false"  />
            </td>                  
        </tr>
        <tr>
            <td>SVO No</td>
            <td>:</td>
            <td>    
                <asp:TextBox ID="tbSvoNo" runat="server" CssClass="TextBox" Enabled="False" 
                    EnableTheming="True" ReadOnly="True" Width="260px" />
            </td>   
            <td>
                <asp:Button ID="btnSvo" runat="server" Class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>
                             
        </tr> 

        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" Width="400px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />
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
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="false">
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
                                <asp:Button ID="btnClosing" runat="server" Class="bitbtndt btnedit"
                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="Closing" 
                                    Text="Closing" />
                                <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                <%--<asp:Button ID="btnDetailPO" runat="server" Class="bitbtndt btngetitem" 
                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="DetailPO" 
                                    Text="Detail PO" Width="70" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" 
                            HeaderText="Product Code" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px" 
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RequireDate" dataformatstring="{0:dd MMM yyyy}" 
                            HeaderStyle-Width="80px" HeaderText="Require Date" htmlencode="true" 
                            SortExpression="RequireDate" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" 
                            HeaderStyle-Width="250px" HeaderText="Spesification" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="80px" 
                            HeaderText="Order Qty " DataFormatString="{0:#,##0.##}" 
                            ItemStyle-HorizontalAlign="Right" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" 
                            HeaderText="Order Unit" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Qty" HeaderText="Qty" 
                            DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" 
                            HeaderStyle-Width="80px" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-Width="80px" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" 
                            HeaderStyle-Width="250px" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DoneClosing" HeaderText="Closing" 
                            HeaderStyle-Width="50px" >
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyPO" DataFormatString="{0:#,##0.##}" HeaderText="PO Qty" />
                        <asp:BoundField DataField="QtyIssue" DataFormatString="{0:#,##0.##}" HeaderText="Issue Qty" />
                        <asp:BoundField DataField="QtyClose" DataFormatString="{0:#,##0.##}" HeaderText="Close Qty" />
                        <asp:BoundField DataField="RemarkClose" HeaderText="Close Remark" />
                    </Columns>
                </asp:GridView>
            </div>
            <br />
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
            <tr>
            <td style="width:60%">            
                <table>
                    <tr>
                        <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbProdCode" CssClass="TextBox" AutoPostBack="true" />
                            <asp:TextBox runat="server"  CssClass="TextBox"
                                ID="tbProdName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="260px"/>
                            <asp:Button Class="btngo" ID="btnProd" Text="..." runat="server" ValidationGroup="Input" />                                               
                            &nbsp;<asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label></tr>                                                    
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td><asp:TextBox TextMode = "MultiLine" runat="server" 
                                CssClass="TextBoxMultiR" Width ="400px" ID="tbSpec" />                                              
                         </td>
                    </tr>
                    <tr>
                    
                        <td>
                            Require Date</td>
                        <td>
                            :</td>
                        <td>
                            <BDP:BasicDatePicker ID="RequireDate" runat="server" AutoPostBack="True" 
                                ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                                TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                <TextBoxStyle CssClass="TextDate" />
                            </BDP:BasicDatePicker>
                        </td>
                    </tr>  
                    <tr>
                            <td>Qty</td>
                            <td>:</td>
                            <td>
                                <table cellspacing="0" cellpadding="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>Order</td>
                                        <td>Order Unit </td>
                                        <td>Wrhs</td>
                                        <td>Wrhs Unit </td>                                
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyreq" 
                                                AutoPostBack="true" Width="96px" /></td>
                                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitReq" runat="server" AutoPostBack="true"/></td>
                                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyWrhs" Width="80px"/></td>
                                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitWrhs" runat="server" 
                                                Enabled="False"/></td>
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
                            <asp:TextBox ID="tbRemarkDt" runat="server" Width="350px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
                </td>
                <td style="vertical-align:top;">
                <table>
                <tr>
                <td>
                    <asp:Panel runat="server" ID="PnlInfo" Visible="true" Height="100%" Width="100%">
                                    <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Stock :"></asp:Label>
                                    <br />
                                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                                        <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="false" ShowFooter="false">
                                            <HeaderStyle CssClass="GridHeader" Height="20px" />
                                            <RowStyle CssClass="GridItem" Wrap="false" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <PagerStyle CssClass="GridPager" />
                                            <Columns>
                                                <asp:BoundField DataField="Warehouse" HeaderStyle-Width="120px" HeaderText="Warehouse" />
                                                <asp:BoundField DataField="Qty" HeaderStyle-Width="70px" HeaderText="Qty" />
                                                <asp:BoundField DataField="QtyMin" HeaderStyle-Width="70px" HeaderText="Qty Min" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
             </td>
            </tr>
            </table>
            </td>
            </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       <br />     
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
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
     <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />
     
    <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    
    </form>
    </body>
</html>
