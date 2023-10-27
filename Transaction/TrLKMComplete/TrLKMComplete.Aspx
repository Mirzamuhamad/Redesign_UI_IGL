<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLKMComplete.Aspx.vb" Inherits="Transaction_TrLKMComplete_TrLKMComplete" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>L K M Complete</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
	

	
	
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label ID="lbltitle" runat="server" Text="Label"></asp:Label> </div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="WoNo" Value="WoNo"></asp:ListItem>
                  <asp:ListItem Text="Divisi Block" Value="DivisiBlok"></asp:ListItem>                            
                  <asp:ListItem Text="DivisiBlockName" Value="DivisiBlokName"></asp:ListItem>                                
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
               <%-- <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        --%>
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
                  <asp:ListItem Selected="true" Text="Trans No" Value="TransNmbr"></asp:ListItem>
                  <asp:ListItem Text="WO No" Value="WoNo"></asp:ListItem>
                  <asp:ListItem Text="DivisiBlock" Value="DivisiBlock"></asp:ListItem>                            
                  <asp:ListItem Text="DivisiBlockName" Value="Divisi Block Name"></asp:ListItem>                                           
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Complete</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlComp" CssClass="DropDownList" Enabled ="false" > 
                    <asp:ListItem Value ="Asisten" >Asisten</asp:ListItem>
                    <asp:ListItem Value ="Audit" >Audit</asp:ListItem>
                    <asp:ListItem Value ="Denda" >Denda</asp:ListItem>
             </asp:DropDownList>
              <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Complete" width="85"/>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnUnApply" Text="UnComplete" width="85"/>
            </td>
        </tr>
        <tr>
        <td >PIC</td>
        <td>:</td>
        <td><asp:TextBox ID="tbPIC" runat="server" CssClass="TextBox" Width="150px" /></td>
        <td>Remark 1</td>
        <td>:</td>
        <td ><asp:TextBox ID="tbRemark1" runat="server" CssClass="TextBox" Width="150px" /></td>
        <td>Remark 2</td>
        <td>:</td>
        <td><asp:TextBox ID="tbRemark2" runat="server" CssClass="TextBox" Width="150px" /></td>
        <td>Remark 3</td>
        <td>:</td>
        <td ><asp:TextBox ID="tbRemark3" runat="server" CssClass="TextBox" Width="150px" /></td>
        </tr>
    </table> 
    <br />
    
     <asp:Panel ID="pnlCompleteAsisten" runat="server" Visible="false">
    <asp:GridView id="DataGrid" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField>
                                  <HeaderTemplate>
                                      <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                                      oncheckedchanged="cbSelectHd_CheckedChanged" />
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:CheckBox ID="cbSelect" AutoPostBack="true" OnCheckedChanged="cbSelect_CheckedChanged" runat="server" />
                                  </ItemTemplate>
                              </asp:TemplateField>
				            <asp:TemplateField HeaderText="Trans No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="TransNmbr" text='<%# DataBinder.Eval(Container.DataItem, "TransNmbr") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="WO No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="WONo" text='<%# DataBinder.Eval(Container.DataItem, "WONo") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Type" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Divisi Block" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisiBlock" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlock") %>'/>
                                    <asp:Label Runat="server" ID="DivisiBlockName" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlockName") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>
							
				            <asp:TemplateField HeaderText="Qty LKM" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyLKM" text='<%# Format( DataBinder.Eval(Container.DataItem, "QtyLKM"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty Asisten" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" CssClass="TextBox" ID="AsistenQty" autopostback="true" OnTextChanged ="AsistenQty_OnTextChanged" Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AsistenQty"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Price" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AsistenPrice"  text='<%# Format(DataBinder.Eval(Container.DataItem, "AsistenPrice"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Bruto" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AsistenBruto"  text='<%# Format(DataBinder.Eval(Container.DataItem, "AsistenBruto"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Disc (%)" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" CssClass="TextBox" ID="AsistenDiscPercent" autopostback="true" OnTextChanged ="AsistenDiscPercent_OnTextChanged"  Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AsistenDiscPercent"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Asisten Disc RP" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" CssClass="TextBox" ID="AsistenDisc" autopostback="true" OnTextChanged ="AsistenDisc_OnTextChanged"   Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AsistenDisc"),"#,##0.00") %>' Width="70" />
								</Itemtemplate>														
							</asp:TemplateField>
							
						    <asp:TemplateField HeaderText="Netto" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AsistenNetto"  text='<%# Format (DataBinder.Eval(Container.DataItem, "AsistenNetto"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
														
							<asp:TemplateField HeaderText="Job" ItemStyle-HorizontalAlign = "Left" SortExpression="JobCode">
								<Itemtemplate>
								<asp:Label runat="server" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "JobCode") %>'/>
								<asp:Label runat="server" ID="JobName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Kontraktor" ItemStyle-HorizontalAlign = "Left" SortExpression="Supplier">
								<Itemtemplate>
								<asp:Label runat="server" ID="Supplier" text='<%# DataBinder.Eval(Container.DataItem, "Supplier") %>'/>
								<asp:Label runat="server" ID="SupplierName" text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Asisten" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAsisten">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAsisten" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAsisten") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Audit" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAudit">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAudit" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAudit") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Denda" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteDenda">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteDenda" text='<%# DataBinder.Eval(Container.DataItem, "CompleteDenda") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
												
    					</Columns>
        </asp:GridView>               
      </asp:Panel>  
      
      <asp:Panel ID="PnlCompleteAudit" runat="server" Visible="false">
    <asp:GridView id="DataGridAudit" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
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
				            <asp:TemplateField HeaderText="Trans No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="TransNmbr" text='<%# DataBinder.Eval(Container.DataItem, "TransNmbr") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="WO No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="WONo" text='<%# DataBinder.Eval(Container.DataItem, "WONo") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Type" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Divisi Block" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisiBlock" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlock") %>'/>
                                    <asp:Label Runat="server" ID="DivisiBlockName" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlockName") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>
							
				            <asp:TemplateField HeaderText="Qty LKM" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyLKM" text='<%# Format( DataBinder.Eval(Container.DataItem, "QtyLKM"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty Audit" >
								<Itemtemplate>
									<asp:TextBox runat="server" cssClass="TextBox" style="text-align: right" ID="AuditQty" autopostback="true" OnTextChanged ="AuditQty_OnTextChanged" Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AuditQty"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Price Audit" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AuditPrice"  text='<%# Format(DataBinder.Eval(Container.DataItem, "AuditPrice"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Bruto Audit" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AuditBruto"  text='<%# Format(DataBinder.Eval(Container.DataItem, "AuditBruto"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Disc Audit" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" cssClass="TextBox" ID="AuditDiscPercent"  Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AuditDiscPercent"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="DiscForex Audit" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" cssClass="TextBox" ID="AuditDisc"  Maxlength = "9" text='<%# Format (DataBinder.Eval(Container.DataItem, "AuditDisc"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
						    <asp:TemplateField HeaderText="Netto Audit" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="AuditNetto"  text='<%# Format (DataBinder.Eval(Container.DataItem, "AuditNetto"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
														
							<asp:TemplateField HeaderText="Job" ItemStyle-HorizontalAlign = "Left" SortExpression="JobCode">
								<Itemtemplate>
								<asp:Label runat="server" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "JobCode") %>'/>
								<asp:Label runat="server" ID="JobName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Kontraktor" ItemStyle-HorizontalAlign = "Left" SortExpression="Supplier">
								<Itemtemplate>
								<asp:Label runat="server" ID="Supplier" text='<%# DataBinder.Eval(Container.DataItem, "Supplier") %>'/>
								<asp:Label runat="server" ID="SupplierName" text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Asisten" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAsisten">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAsisten" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAsisten") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Audit" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAudit">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAudit" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAudit") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Denda" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteDenda">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteDenda" text='<%# DataBinder.Eval(Container.DataItem, "CompleteDenda") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
												
    					</Columns>
        </asp:GridView>               
      </asp:Panel>   
      
       <asp:Panel ID="PnlCompleteDenda" runat="server" Visible="false">
    <asp:GridView id="DataGridDenda" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
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
				            <asp:TemplateField HeaderText="Trans No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="TransNmbr" text='<%# DataBinder.Eval(Container.DataItem, "TransNmbr") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="WO No" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="WONo" text='<%# DataBinder.Eval(Container.DataItem, "WONo") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Type" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Divisi Block" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="DivisiBlock" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlock") %>'/>
                                    <asp:Label Runat="server" ID="DivisiBlockName" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlockName") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Denda Amount" >
								<Itemtemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="DendaAmount"  text='<%# Format (DataBinder.Eval(Container.DataItem, "DendaAmount"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Manager" >
								<Itemtemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="DendaManager" autopostback="true" OnTextChanged ="DendaManager_OnTextChanged"  text='<%# DataBinder.Eval(Container.DataItem, "DendaManager") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Manager Name" >
								<Itemtemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="DendaManagerName"   text='<%# DataBinder.Eval(Container.DataItem, "DendaManagerName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Manager Percent" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaManagerPercent" CssClass="TextBox"  text='<%# Format(DataBinder.Eval(Container.DataItem, "DendaManagerPercent"),"#,##0.00")  %>' Width="40"/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Askep" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: Left" CssClass="TextBox" ID="DendaAskep" autopostback="true" OnTextChanged ="DendaAskep_OnTextChanged" Maxlength = "12" text='<%# DataBinder.Eval(Container.DataItem, "DendaAskep") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Denda Askep Name" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: Left" CssClass="TextBox" ID="DendaAskepName" text='<%# DataBinder.Eval(Container.DataItem, "DendaAskepName") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Askep Percent" >
								<Itemtemplate>
									<asp:TextBox runat="server" style="text-align: right" CssClass="TextBox" ID="DendaAskepPercent"   text='<%# Format (DataBinder.Eval(Container.DataItem, "DendaAskepPercent"),"#,##0.00") %>' Width="40" />
								</Itemtemplate>														
							</asp:TemplateField>
							
						    <asp:TemplateField HeaderText="Denda Asisten" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaAsisten" CssClass="TextBox" autopostback="true" OnTextChanged ="DendaAsisten_OnTextChanged" text='<%#DataBinder.Eval(Container.DataItem, "DendaAsisten") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							 <asp:TemplateField HeaderText="Denda Asisten Name" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaAsistenName" CssClass="TextBox" text='<%#DataBinder.Eval(Container.DataItem, "DendaAsistenName") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Asisten Percent" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaAsistenPercent" CssClass="TextBox" text='<%# Format (DataBinder.Eval(Container.DataItem, "DendaAsistenPercent"),"#,##0.00") %>' Width="40"/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Mandor" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaMandor" CssClass="TextBox" autopostback="true" OnTextChanged ="DendaMandor_OnTextChanged" text='<%# DataBinder.Eval(Container.DataItem, "DendaMandor") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Mandor Name" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaMandorName" CssClass="TextBox" text='<%# DataBinder.Eval(Container.DataItem, "DendaMandorName") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Denda Mandor Percent" >
								<Itemtemplate>
									<asp:TextBox Runat="server" ID="DendaMandorPercent" CssClass="TextBox" text='<%# Format (DataBinder.Eval(Container.DataItem, "DendaMandorPercent"),"#,##0.00") %>' Width="40"/>
								</Itemtemplate>														
							</asp:TemplateField>
														
							<asp:TemplateField HeaderText="Asisten" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAsisten">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAsisten" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAsisten") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Audit" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteAudit">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteAudit" text='<%# DataBinder.Eval(Container.DataItem, "CompleteAudit") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Denda" ItemStyle-HorizontalAlign = "Left" SortExpression="CompleteDenda">
								<Itemtemplate>
								<asp:Label runat="server" ID="CompleteDenda" text='<%# DataBinder.Eval(Container.DataItem, "CompleteDenda") %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
												
    					</Columns>
        </asp:GridView>               
      </asp:Panel> 
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
	<asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
</body>
</html>
