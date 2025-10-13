<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsJobMaterial.Aspx.vb" Inherits="Master_MsJobMaterial_MsJobMaterial" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Material File</title>    
    <script src="../../Function/Function.JS" type="text/javascript"></script>
     <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
     <script type="text/javascript">
         function OpenPopup() {
             window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
             return false;
         }
         function openprintdlg() {
             var wOpens;
             wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
             wOpens.moveTo(0, 0);
             wOpens.resizeTo(screen.width, screen.height);
         }

         function OpenPopup() {
             window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
             return false;
         }    
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Job Material File</div>
    <hr style="color:Blue" />    
    <asp:Panel id="pnlHd" runat="server">
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>                          
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" 
                    Visible="False"/>        
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
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>                      
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <%--<td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    
            </td>--%>
            
            <td>Job Group</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlJobGroup" CssClass="DropDownList" AutoPostBack="true">                        
                </asp:DropDownList></td>
            <td>            
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
				            <asp:TemplateField HeaderText="Job Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobCode" text='<%# DataBinder.Eval(Container.DataItem, "JobCode") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Status Tanam">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StatusTanam" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanam") %>' Width="100" Visible="False" />
									<asp:Label Runat="server" ID="StatusTanamName" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanamName") %>' Width="100" />
								</Itemtemplate>																
							</asp:TemplateField>								
							
							
							<asp:TemplateField HeaderText="Convertion" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyConvert">
								<Itemtemplate>
								<asp:Label runat="server" ID="QtyConvert" text='<%# Format (DataBinder.Eval(Container.DataItem, "QtyConvert"),"#,##0.00") %>' Width="40" />
								<asp:Label Runat="server" ID="lbUnitC" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								    <asp:Label Runat="server" ID="Label1" text="/"/>
								    <asp:Label runat="server" ID="QtyAreal" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "QtyAreal") %>' Width="20px" />
								    <asp:Label Runat="server" ID="lbUnitA" text='<%# DataBinder.Eval(Container.DataItem, "UnitAreal") %>'/>
								</Itemtemplate>		
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>

							
							<asp:TemplateField HeaderText="X Rotation" ItemStyle-HorizontalAlign = "Right" SortExpression="XRotation">
								<Itemtemplate>
								<asp:Label runat="server" ID="XRotation" text='<%# DataBinder.Eval(Container.DataItem, "XRotation") %>' Width="40" />
								</Itemtemplate>		
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>								    
								    <asp:Button ID="btnPercentage" runat="server" class="bitbtndt btndetail" Text="Percentage" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Percentage" Width="80" />								
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Material" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Detail" />
								</Itemtemplate>		
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>

																		
    					</Columns>
        </asp:GridView>               
    </asp:Panel>
    <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <table>
       <tr>
       <td>
            <asp:Label ID="lblJobDt" CssClass="H1" runat="server" Text="" />             
       </td>
       <td>
       </td>
            &nbsp &nbsp &nbsp &nbsp &nbsp
       <td>
            <asp:Label ID="lblStatusTanamDt" CssClass="H1" runat="server" Text="" />             
       </td>
       </tr>
       <tr>
            <td>
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            </td>  
            <td>
               <%-- <asp:Label ID="label2" CssClass="H1" runat="server" Text="Total : " />             
                <asp:Label ID="lblTotal" CssClass="H1" runat="server" Text="XXX " />             
                <asp:Label ID="label3" CssClass="H1" runat="server" Text="%" />         --%>    
            </td>
            <td>
            </td>
       </tr>
     </table>
     
     
	 <br />
	 
	 
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView ID="DataGridDt" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="false">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign = "Right" >
                        <Itemtemplate>
                            <asp:Label ID="RotasiNo" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "RotasiNo") %>' />
                        </Itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Percentage" SortExpression="RMPercentage">
                        <ItemTemplate>
                            <asp:TextBox ID="RMPercentage" Runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "RMPercentage") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <br />
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
     </asp:Panel>
     <asp:Panel ID="pnlViewDt" runat="server" Visible = "False">  
     <table>
     <tr>
     <td Width="70%">
     
            <asp:Label ID="lblJobMaterial" CssClass="H1" runat="server" Text=" " />             
       </td>
       <td>
       </td>
            &nbsp &nbsp &nbsp &nbsp &nbsp
       <td>
            <asp:Label ID="lblStatusTanamMaterial" CssClass="H1" runat="server" Text=" " />             
      
     
     </td>
     
     </tr>
     </table>     
     
	 <br />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView ID="DataGridMaterial" runat="server" AllowPaging="false" 
                AllowSorting="false" AutoGenerateColumns="False" ShowFooter="True">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridFooter" Wrap="false" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
					
                    <asp:TemplateField HeaderStyle-Width="128" HeaderText="Product Code" 
                        SortExpression="ProductCode">
                        <Itemtemplate>
                            <asp:Label ID="ProductCode" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountEdit" Text="..." runat="server" CommandName="SearchAccountEdit" />--%>
                            <asp:Label ID="ProductCodeEdit" runat="server" AutoPostBack="true" Maxlength = "12" 
                                CssClass="TextBox" ontextchanged="tbProductCode_TextChanged" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>' Width="90" />
                            <asp:Button ID="btnProductCode" runat="server" class="btngo" Text="..." CommandName="SearchProductCodeEdit"/>                            
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountAdd" Text="..." runat="server" CommandName="SearchAccountAdd" />--%>
                            <asp:TextBox ID="ProductCodeAdd" runat="server" AutoPostBack="true" Text = "" 
                                CssClass="TextBox" ontextchanged="tbProductCode_TextChanged" Width="90" MaxLength="12" />
                            <asp:Button ID="btnProductCodeAdd" runat="server" class="btngo" Text="..." CommandName="SearchProductCodeAdd"/>                            
                            </FooterTemplate>
                        <HeaderStyle Width="128" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-Width="250" HeaderText="Product Name" 
                        SortExpression="ProductName">
                        <Itemtemplate>
                            <asp:Label ID="ProductName" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="ProductNameEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>' Width="250%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="ProductNameAdd" Runat="server" Width="250"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="250" />                       
                    </asp:TemplateField>   
                    	
                    <asp:TemplateField HeaderText="Qty Dosis/Tahun" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
						       <Itemtemplate >
									<asp:Label Runat="server" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty","{0:#,##.00}") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="QtyEdit" Width="95" MaxLength="22" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Qty","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="Unit">
						       <Itemtemplate >
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="UnitEdit" Width="95" MaxLength="22" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Unit","{0:#,##}") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="UnitAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= ""/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
					
					<asp:TemplateField HeaderText="Qty Alokasi (Pkk)" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Right" SortExpression="QtyCap">
						       <Itemtemplate>
									<asp:Label Runat="server" ID="QtyCap"  text='<%# DataBinder.Eval(Container.DataItem, "QtyCap","{0:#,##.00}") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="QtyCapEdit" Width="95" CssClass="TextBox"  MaxLength="22" Text='<%# DataBinder.Eval(Container.DataItem, "QtyCap","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyCapAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
					
							                   
                    
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" 
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Update" Text="Update" ID="btnUpdate" Runat="server" Width="60" />--%>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save"  CommandName="Update"/>                                                        
                            <%--<asp:Button CssClass="Button" CommandName="Cancel" Text="Cancel" ID="btnCancel" Runat="server" Width="50" />--%>
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btnsave" Text="Cancel"  CommandName="Cancel"/>                                                                                    
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" Width="95" />--%>
                            <asp:Button ID="btnAdd0" runat="server" class="bitbtndt btnadd" Text="Add"  CommandName="Insert"/>                                                                                                                
                        </FooterTemplate>
                        <ItemTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Edit" Text="Edit" ID="btnEdit" Runat="server" Width="50" />--%>
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit"  CommandName="Edit"/>                                                                                                                
                            <%--<asp:Button CssClass="Button" CommandName="Delete" Text="Delete" ID="btnDelete" OnClientClick="return confirm('Sure to delete this data?');" Runat="server" Width="55" />--%>
                            <asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>                                                                                                                                           
                        </ItemTemplate>
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
	    
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnBack2" Text="Back" />
	       				                        
     </asp:Panel>
     <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    
    
    
    
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>       
    <br />
    
    
    
    </form>
</body>
</html>
