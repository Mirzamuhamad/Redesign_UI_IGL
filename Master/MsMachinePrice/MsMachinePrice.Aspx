<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachinePrice.Aspx.vb" Inherits="Master_MsMachinePrice_MsMachinePrice" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Machine Price File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Machine Price File</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Machine Code" Value="A.Machine"></asp:ListItem>
                  <asp:ListItem Text="Machine Name" Value="A.MachineName"></asp:ListItem>                            
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
                      <asp:ListItem Selected="true" Text="Machine Code" Value="A.Machine"></asp:ListItem>
                      <asp:ListItem Text="Machine Name" Value="A.MachineName"></asp:ListItem>                      
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
             <td style="width:100px;text-align:right" >Machine Group :</td>
            <td><asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownList" AutoPostBack= "true" >                            
             </asp:DropDownList> </td>
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
				            <asp:TemplateField HeaderText="Machine Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Machine" text='<%# DataBinder.Eval(Container.DataItem, "Machine") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Machine Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineName" text='<%# DataBinder.Eval(Container.DataItem, "MachineName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						
							<asp:TemplateField HeaderText="Last Date" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="tbLastDate" text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPrice" style="text-align: right" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign = "Right" SortExpression="Price">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Price" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Last Premi" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPremi" style="text-align: right" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Premi"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Premi" ItemStyle-HorizontalAlign = "Right" SortExpression="Premi">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Premi" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Premi"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Unit" ItemStyle-HorizontalAlign = "Right" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
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
