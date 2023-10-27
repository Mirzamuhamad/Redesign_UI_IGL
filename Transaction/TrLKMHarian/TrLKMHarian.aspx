<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLKMHarian.aspx.vb" Inherits="Transaction_TrLKMHarian_TrLKMHarian" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LKM Closing Harian</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
	<script src="../../Function/easy.qrcode.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    
    <%--<script type="text/javascript">
        function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
        }    
    </script>--%>
	
	
	
	
	
	
	<script type="text/javascript" id="qrcodeTpl">			
				<div class="qr" id="qrcode_{i}"></div>
</script>

		
	
	
	
    <style type="text/css">
        .style2
        {
            width: 92px;
            text-align: right;
            height: 24px;
        }
        .style3
        {
            height: 24px;
        }
		
		
        </style>
</head>
<body>

    <form id="form1" runat="server">
    <div class="Content">
	
	    <div id="container">
			<asp:TextBox ID="txtCode" value = "Tes Code" runat="server"></asp:TextBox>
        <asp:Button ID="btnGenerate" runat="server" Text="Generate" onclick="btnGenerate_Click" />
		<asp:PlaceHolder ID="plBarCode" runat="server" />
        <hr />
         <input id="btnprint" type="button" onclick="PrintDiv()" value="Print" /></center>
		</div>
		

        

        <div class="H1">&nbsp;LKM Closing Harian</div>
        <hr style="color: Blue" />
        
        <asp:Panel ID="PnlMain" runat="server">
        
            <table>
                <tr>
                    <%--<td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="EmpID" Selected="True">EmployNo</asp:ListItem>
                            <asp:ListItem Value="EmpName">Employ Name</asp:ListItem>
                            <asp:ListItem Value="JobTitle">Job Title</asp:ListItem>
                            <asp:ListItem Value="LKMDay">Days</asp:ListItem>
                            <asp:ListItem Value="LKMCekDay">Cek Days</asp:ListItem>
                            <asp:ListItem Value="LKMNotCekDay">Not Cek Days</asp:ListItem>
                            <asp:ListItem Value="LKMHK">Hk Valid</asp:ListItem>
                        </asp:DropDownList>                       
                        
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <%--<asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />--%>
                        <asp:Button class="bitbtn btnprint" runat="server" ID="BtnPrint" Text="Print" Visible="false"/>
                    </td>
                    <%--<td class="style4">
                        Type :
                    </td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlType" AutoPostBack="true">
                            <asp:ListItem>Service</asp:ListItem>
                            <asp:ListItem>Trading</asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                </tr>
            </table>
            
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2">
                            <asp:ListItem Value="EmpID" Selected="True">EmployNo</asp:ListItem>
                            <asp:ListItem Value="EmpName">Employ Name</asp:ListItem>
                            <asp:ListItem Value="JobTitle">Job Title</asp:ListItem>
                            <asp:ListItem Value="LKMDay">Days</asp:ListItem>
                            <asp:ListItem Value="LKMCekDay">Cek Days</asp:ListItem>
                            <asp:ListItem Value="LKMNotCekDay">Not Cek Days</asp:ListItem>
                            <asp:ListItem Value="LKMHK">Hk Valid</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table>
                <tr>
                    <td class="style2">
                        EmpID</td>
                    <td class="style3">
                        :
                    </td>
                    <td class="style3">
                        <asp:Dropdownlist ID="ddlStartWeek" Enabled="False" runat="server" CssClass="TextBox" Height="16px" 
                            Width="144px" />
                        &nbsp;&nbsp;&nbsp; s/d&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlEndWeek" AutoPostBack ="True" runat="server" CssClass="TextBox" Height="16px" 
                            Width="144px" />
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td class="style3">
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="BtnApply" Text=".." 
                            Visible="true" Height="20px" Width="20px"/>
                        &nbsp;&nbsp;&nbsp; Division :&nbsp;
                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="True" 
                            CssClass="TextBox" Height="16px" Width="132px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td class="style3">
                        <asp:Button class="bitbtndt btnclose" runat="server" ID="btnClosing" 
                            Text="Closing" Visible="true" Width="62px"/>
                        <%--&nbsp--%>
                        <%--<asp:Button class="bitbtn btnprint" runat="server" ID="BtnPrint" Text="Print" />--%>
                        <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
                    </td>
                    <td class="style3">
                        <asp:Button class="bitbtn btnunclose" runat="server" ID="BtnUnClosing" 
                            Text="Un-Closing" Visible="true" Width="82px"/>
                    </td>
                    
                </tr>
               
            </table>
        </asp:Panel>
        
        <br />
        <div class="H1"> Detail LKM</div>
        <asp:Panel runat="server" ID="pnlService" Visible="false">
            <asp:GridView ID="DataGrid" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="True" Width="654px">
                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                  
                    <asp:TemplateField HeaderText="Employe No">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="EmpID" Text='<%# DataBinder.Eval(Container.DataItem, "EmpID") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Employe Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="WeekNo" Text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Job Title">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    
                   <%--<asp:TemplateField HeaderText="Job Title" HeaderStyle-Width="220" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" HeaderStyle-Width="220" ID="tbStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="220px" />
                   </asp:TemplateField>--%>
                    
                    <asp:BoundField DataField="LKMDay" 
                        HeaderStyle-Width="120px" SortExpression="StartDate" HeaderText="Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="LKMCekDay" 
                        HeaderStyle-Width="120px" SortExpression="StartDate" HeaderText="Cek Days Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    
                    <%--<asp:TemplateField HeaderText="Cek Days" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbEndDate" Text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:BoundField DataField="LKMNotCekDay" 
                        HeaderStyle-Width="120px" SortExpression="EndDate" HeaderText="Not Cek Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LKMHK" 
                        HeaderStyle-Width="120px" SortExpression="EndDate" HeaderText="HK Valid">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    
                     
                    <%--<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LastPrice" Text='<%# DataBinder.Eval(Container.DataItem, "LastPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
                    
                    <%--<asp:TemplateField HeaderText="Action" >
						<ItemTemplate>		
						    <asp:Button ID="btnApply" runat="server" class="bitbtndt btnedit" Text="Apply" CommandName="Edit"/>										             
						</ItemTemplate>
					</asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
            <br />
        </asp:Panel>
        <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
        <asp:Panel ID="pnlInput" runat="server" Visible="false">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="True" Width="654px">
                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    
                    <asp:TemplateField HeaderText="Employe No">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="EmpID" Text='<%# DataBinder.Eval(Container.DataItem, "EmpID") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Employe Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="WeekNo" Text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Job Title">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "JobTitle") %>' />                            
                        </ItemTemplate>
                        <HeaderStyle Width="200px" />
                    </asp:TemplateField>
                    
                  
                    
                    <asp:BoundField DataField="LKMDay"
                        HeaderStyle-Width="120px" SortExpression="StartDate" HeaderText="Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="LKMCekDay" 
                        HeaderStyle-Width="120px" SortExpression="StartDate" HeaderText="Cek Days Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>

                    
                    <asp:BoundField DataField="LKMNotCekDay"
                        HeaderStyle-Width="120px" SortExpression="EndDate" HeaderText="Not Cek Days">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LKMHK"
                        HeaderStyle-Width="120px" SortExpression="EndDate" HeaderText="HK Valid">
                        <HeaderStyle Width="120px" />
                    </asp:BoundField>
                    
                </Columns>
            </asp:GridView>
            <br />
        <table>
           
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" class="bitbtn btnunclose" Text="YES" /> &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtn btnclose" Text="NO" CommandName="Cancel"/> &nbsp;
                    <%--<asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>--%>       
                 </td>
            </tr>
        </table>
     </asp:Panel>
        
        
        <br />
        
    </div>
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    <%-- <asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LastPrice" Text='<%# DataBinder.Eval(Container.DataItem, "LastPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField> --%>
    </form>
</body>

<script type="text/javascript">

 function PrintDiv() {
                var divToPrint = document.getElementById('Container');
                var popupWin = window.open('', '_blank', 'width=500,height=400,location=no,left=500px');
                popupWin.document.open();
                popupWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</html>');
                popupWin.document.close();
            }


			var demoParams = [
				
				{
					config: {
						text: document.getElementById('txtCode').value ,
						width: 200,
						height: 200,
						quietZone: 10,
						colorDark: "#000000",
						colorLight: "#ffffff",
						//PI: '#f55066',
						correctLevel: QRCode.CorrectLevel.H // L, M, Q, H
					}
				}
            ]
            
			var qrcodeTpl = document.getElementById("qrcodeTpl").innerHTML;
			var container = document.getElementById('container');

			for (var i = 0; i < demoParams.length; i++) {
				var qrcodeHTML = qrcodeTpl.replace(/\{title\}   /, demoParams[i].title).replace(/{i}/, i);
				container.innerHTML+=qrcodeHTML;
			}
			for (var i = 0; i < demoParams.length; i++) {
				 var t = new QRCode(document.getElementById("qrcode_"+i), demoParams[i].config);
			}
		</script>
</html>
