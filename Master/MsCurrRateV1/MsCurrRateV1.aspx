<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCurrRateV1.Aspx.vb" Inherits="Master_MsCurrRateV1_MsCurrRateV1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Currency Rate File</title>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../Master/MsCurrRateViewV1/MsCurrRateViewV1.Aspx","List","scrollbars=yes,resizable=no,width=700,height=500");        
        return false;
    }    
    
    function Confirm() {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_value";
    
    var ddlCommand = document.getElementById("btnDelete")
    
    if (confirm("Are you sure you want to Delete?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "Tidak";
        }
        document.forms[0].appendChild(confirm_value);
    }
   
   function postback()
        {
            __doPostBack('','');
        }
   </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <style type="text/css">
        .style1
        {
            width: 354px;
        }
        .style2
        {
            width: 109px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Currency Rate File</div>
    <hr style="color:Blue" />    
    <table>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbStartDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" Enabled="false">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
            </td>
            <td>&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;</td>
            <td><BDP:BasicDatePicker ID="tbEndDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
            </td>
            <td><asp:Button class="bitbtn btnadd" runat="server" ID="btnNew" Text="New" /></td>
            <td><asp:Button class="bitbtn btnapply" runat="server" ID="btnApply" Text="Apply" /></td>
            <td><asp:Button class="bitbtn btnsearch" runat="server" ID="btnView" Text="View History" Width="91px" /></td>
            <td><asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" /></td> <%--OnClick="OnConfirm" OnClientClick="Confirm()"--%>
        </tr>
    </table> 
    <asp:Panel runat="server" ID="pnlView" Visible="false">
        <table style="width: 620px">
            <tr>
            <td class="style1"></td>
                <td class="style2">
                <fieldset runat="server" id="fsRpt" style="width:237px">
                <legend runat="server" id="lgRpt">Choose Periode</legend>
                    &nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlPeriode" runat="server" CssClass="DropDownList" Width="186px" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnOK" runat="server" class="bitbtn btngo" Text="OK" Width="22px" />
                </fieldset>
                </td>
            </tr>
        </table>     
    </asp:Panel>
    <br />
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" CssClass="Grid" Width="592px">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="FgAdd" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgAdd" text='<%# DataBinder.Eval(Container.DataItem, "FgAdd") %>'/>
								</Itemtemplate>															
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Currency Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CurrCode" text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Currency Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CurrName" text='<%# DataBinder.Eval(Container.DataItem, "CurrName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						
							<%--<asp:TemplateField HeaderText="Last Date">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="tbLastDate" text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="Operator" >
								<Itemtemplate>
									<asp:DropDownList CssClass="DropDownList" ID="Operator" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Operator") %>'>
                                        <asp:ListItem>x</asp:ListItem>
                                        <asp:ListItem>/</asp:ListItem>                                        
                                    </asp:DropDownList>
								</Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Currency Rate" ItemStyle-HorizontalAlign = "Right" SortExpression="CurrRate">
								<Itemtemplate>
									<asp:TextBox runat="server" CssClass="TextBox" ID="CurrRate" Maxlength = "9" OnTextChanged= "CurrRate_TextChanged"   AutoPostBack="true" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "CurrRate"))) %>'/>
								</Itemtemplate>		

<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Tax Rate" ItemStyle-HorizontalAlign = "Right" SortExpression="TaxRate">
								<Itemtemplate>
									<asp:TextBox runat="server" ID="TaxRate" Maxlength = "9" CssClass="TextBox" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "TaxRate"))) %>'/>
								</Itemtemplate>	

<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>												
    					</Columns>
        </asp:GridView>       
    </div>
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
