<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCurrRate.Aspx.vb" Inherits="Master_MsCurrRate_MsCurrRate" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Currency Rate File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Currency Rate File</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Currency Code" Value="A.CurrCode"></asp:ListItem>
                  <asp:ListItem Text="Currency Name" Value="A.CurrName"></asp:ListItem>                            
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                      <asp:ListItem Selected="true" Text="Currency Code" Value="A.CurrCode"></asp:ListItem>
                      <asp:ListItem Text="Currency Name" Value="A.CurrName"></asp:ListItem>                      
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
            </td>
            <td>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
            </td>
        </tr>
    </table> 
    <br />
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
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
						
							<asp:TemplateField HeaderText="Last Date" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="tbLastDate" text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>
							
							<%--<asp:TemplateField HeaderText="Operator" >
								<Itemtemplate>
									<asp:DropDownList CssClass="DropDownList" ID="Operator" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Operator") %>'>
                                        <asp:ListItem>x</asp:ListItem>
                                        <asp:ListItem>/</asp:ListItem>                                        
                                    </asp:DropDownList>
								</Itemtemplate>									
							</asp:TemplateField>--%>

							<asp:TemplateField HeaderText="Currency Rate" ItemStyle-HorizontalAlign = "Right" SortExpression="CurrRate">
								<Itemtemplate>
									<asp:TextBox runat="server" CssClass="TextBox" ID="CurrRate" Maxlength = "9" OnTextChanged= "CurrRate_TextChanged"   AutoPostBack="true" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "CurrRate"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Tax Rate" ItemStyle-HorizontalAlign = "Right" SortExpression="TaxRate">
								<Itemtemplate>
									<asp:TextBox runat="server" ID="TaxRate" Maxlength = "9" CssClass="TextBox" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "TaxRate"))) %>'/>
								</Itemtemplate>	
							</asp:TemplateField>												
    					</Columns>
        </asp:GridView>               
    </div>
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>     
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
