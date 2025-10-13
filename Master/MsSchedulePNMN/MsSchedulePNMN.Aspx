<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSchedulePNMN.Aspx.vb" Inherits="Master_MsSchedulePNMN_MsSchedulePNMN" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule PNMN File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="H1">Schedule PNMN File</div>
    <hr style="color:Blue" />    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobPlant"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Job Group" Value="G.JobPlantGrpName"></asp:ListItem>                          
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
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobPlant"></asp:ListItem>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Job Group" Value="G.JobPlantGrpName"></asp:ListItem>                          
                </asp:DropDownList>                                    
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
      <ContentTemplate>      
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
            
            <td>Schedule Type </td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList" AutoPostBack="true">                        
                </asp:DropDownList></td>
            <td>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
            </td>
            <td></td>
            <td>Delete Schedule Type</td>
            <td>:</td>
            <td><asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>	</td>
            <td></td>
            <td> Add Schedule Type</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID ="tbAddSchedule" CssClass="TextBox"/> </td>
            <td> <asp:Button class="btngo" runat="server" ID="btnGoAdd" Text="G"/></td>
            
            <td></td>
            <td>Copy
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlCopyType" >
                    <asp:ListItem Selected="true" Text="From" Value="From"></asp:ListItem>
                    <asp:ListItem Text="To" Value="To"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlCopyScheduleType" CssClass="DropDownList">
                </asp:DropDownList>
            </td>
            <td> <asp:Button class="btngo" runat="server" ID="btnGoCopy" Text="G" /></td>
        </tr>
    </table> 
    <br />
    
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" CssClass="Grid" Width="170px">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				       <asp:TemplateField HeaderText="Job Group">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobGroup" text='<%# DataBinder.Eval(Container.DataItem, "JobPlantGrpName") %>' Width="140" />
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Job Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobCode" text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CustomerName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>' Width="170" />
								</Itemtemplate>																
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Rotation">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Rotation" text='<%# DataBinder.Eval(Container.DataItem, "Rotation") %>' Width="30" />
								</Itemtemplate>																
							</asp:TemplateField>							
						
							<asp:TemplateField HeaderText="Done Rotation">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="DoneRotasi" text='<%# DataBinder.Eval(Container.DataItem, "DoneRotasi") %>' Width="30" />
								</Itemtemplate>																								
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Wk 1">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW01" Checked='<%# DataBinder.Eval(Container.DataItem, "W01") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek" />
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 2">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW02" Checked='<%# DataBinder.Eval(Container.DataItem, "W02") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 3">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW03" Checked='<%# DataBinder.Eval(Container.DataItem, "W03") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 4">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW04" Checked='<%# DataBinder.Eval(Container.DataItem, "W04") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 5">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW05" Checked='<%# DataBinder.Eval(Container.DataItem, "W05") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 6">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW06" Checked='<%# DataBinder.Eval(Container.DataItem, "W06") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 7">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW07" Checked='<%# DataBinder.Eval(Container.DataItem, "W07") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 8">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW08" Checked='<%# DataBinder.Eval(Container.DataItem, "W08") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 9">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW09" Checked='<%# DataBinder.Eval(Container.DataItem, "W09") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 10">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW10" Checked='<%# DataBinder.Eval(Container.DataItem, "W10") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Wk 11">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW11" Checked='<%# DataBinder.Eval(Container.DataItem, "W11") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 12">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW12" Checked='<%# DataBinder.Eval(Container.DataItem, "W12") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 13">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW13" Checked='<%# DataBinder.Eval(Container.DataItem, "W13") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 14">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW14" Checked='<%# DataBinder.Eval(Container.DataItem, "W14") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 15">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW15" Checked='<%# DataBinder.Eval(Container.DataItem, "W15") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 16">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW16" Checked='<%# DataBinder.Eval(Container.DataItem, "W16") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 17">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW17" Checked='<%# DataBinder.Eval(Container.DataItem, "W17") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 18">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW18" Checked='<%# DataBinder.Eval(Container.DataItem, "W18") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 19">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW19" Checked='<%# DataBinder.Eval(Container.DataItem, "W19") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 20">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW20" Checked='<%# DataBinder.Eval(Container.DataItem, "W20") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 21">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW21" Checked='<%# DataBinder.Eval(Container.DataItem, "W21") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 22">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW22" Checked='<%# DataBinder.Eval(Container.DataItem, "W22") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 23">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW23" Checked='<%# DataBinder.Eval(Container.DataItem, "W23") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 24">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW24" Checked='<%# DataBinder.Eval(Container.DataItem, "W24") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 25">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW25" Checked='<%# DataBinder.Eval(Container.DataItem, "W25") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 26">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW26" Checked='<%# DataBinder.Eval(Container.DataItem, "W26") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 27">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW27" Checked='<%# DataBinder.Eval(Container.DataItem, "W27") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 28">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW28" Checked='<%# DataBinder.Eval(Container.DataItem, "W28") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 29">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW29" Checked='<%# DataBinder.Eval(Container.DataItem, "W29") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 30">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW30" Checked='<%# DataBinder.Eval(Container.DataItem, "W30") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 31">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW31" Checked='<%# DataBinder.Eval(Container.DataItem, "W31") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 32">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW32" Checked='<%# DataBinder.Eval(Container.DataItem, "W32") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 33">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW33" Checked='<%# DataBinder.Eval(Container.DataItem, "W33") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 34">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW34" Checked='<%# DataBinder.Eval(Container.DataItem, "W34") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 35">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW35" Checked='<%# DataBinder.Eval(Container.DataItem, "W35") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 36">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW36" Checked='<%# DataBinder.Eval(Container.DataItem, "W36") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 37">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW37" Checked='<%# DataBinder.Eval(Container.DataItem, "W37") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 38">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW38" Checked='<%# DataBinder.Eval(Container.DataItem, "W38") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 39">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW39" Checked='<%# DataBinder.Eval(Container.DataItem, "W39") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 40">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW40" Checked='<%# DataBinder.Eval(Container.DataItem, "W40") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 41">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW41" Checked='<%# DataBinder.Eval(Container.DataItem, "W41") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 42">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW42" Checked='<%# DataBinder.Eval(Container.DataItem, "W42") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 43">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW43" Checked='<%# DataBinder.Eval(Container.DataItem, "W43") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 44">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW44" Checked='<%# DataBinder.Eval(Container.DataItem, "W44") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 45">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW45" Checked='<%# DataBinder.Eval(Container.DataItem, "W45") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 46">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW46" Checked='<%# DataBinder.Eval(Container.DataItem, "W46") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
								    <asp:TemplateField HeaderText="Wk 47">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW47" Checked='<%# DataBinder.Eval(Container.DataItem, "W47") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
									<asp:TemplateField HeaderText="Wk 48">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW48" Checked='<%# DataBinder.Eval(Container.DataItem, "W48") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 49">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW49" Checked='<%# DataBinder.Eval(Container.DataItem, "W49") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 50">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW50" Checked='<%# DataBinder.Eval(Container.DataItem, "W50") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 51">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW51" Checked='<%# DataBinder.Eval(Container.DataItem, "W51") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 52">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW52" Checked='<%# DataBinder.Eval(Container.DataItem, "W52") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 53">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW53" Checked='<%# DataBinder.Eval(Container.DataItem, "W53") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 54">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW54" Checked='<%# DataBinder.Eval(Container.DataItem, "W54") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 55">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW55" Checked='<%# DataBinder.Eval(Container.DataItem, "W55") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 56">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW56" Checked='<%# DataBinder.Eval(Container.DataItem, "W56") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 57">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW57" Checked='<%# DataBinder.Eval(Container.DataItem, "W57") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 58">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW58" Checked='<%# DataBinder.Eval(Container.DataItem, "W58") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 59">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW59" Checked='<%# DataBinder.Eval(Container.DataItem, "W59") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 60">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW60" Checked='<%# DataBinder.Eval(Container.DataItem, "W60") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 61">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW61" Checked='<%# DataBinder.Eval(Container.DataItem, "W61") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 62">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW62" Checked='<%# DataBinder.Eval(Container.DataItem, "W62") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 63">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW63" Checked='<%# DataBinder.Eval(Container.DataItem, "W63") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 64">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW64" Checked='<%# DataBinder.Eval(Container.DataItem, "W64") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 65">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW65" Checked='<%# DataBinder.Eval(Container.DataItem, "W65") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 66">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW66" Checked='<%# DataBinder.Eval(Container.DataItem, "W66") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 67">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW67" Checked='<%# DataBinder.Eval(Container.DataItem, "W67") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 68">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW68" Checked='<%# DataBinder.Eval(Container.DataItem, "W68") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 69">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW69" Checked='<%# DataBinder.Eval(Container.DataItem, "W69") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 70">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW70" Checked='<%# DataBinder.Eval(Container.DataItem, "W70") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 71">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW71" Checked='<%# DataBinder.Eval(Container.DataItem, "W71") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 72">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW72" Checked='<%# DataBinder.Eval(Container.DataItem, "W72") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 73">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW73" Checked='<%# DataBinder.Eval(Container.DataItem, "W73") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 74">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW74" Checked='<%# DataBinder.Eval(Container.DataItem, "W74") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 75">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW75" Checked='<%# DataBinder.Eval(Container.DataItem, "W75") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 76">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW76" Checked='<%# DataBinder.Eval(Container.DataItem, "W76") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 77">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW77" Checked='<%# DataBinder.Eval(Container.DataItem, "W77") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 78">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW78" Checked='<%# DataBinder.Eval(Container.DataItem, "W78") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 79">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW79" Checked='<%# DataBinder.Eval(Container.DataItem, "W79") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 80">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW80" Checked='<%# DataBinder.Eval(Container.DataItem, "W80") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 81">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW81" Checked='<%# DataBinder.Eval(Container.DataItem, "W81") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 82">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW82" Checked='<%# DataBinder.Eval(Container.DataItem, "W82") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 83">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW83" Checked='<%# DataBinder.Eval(Container.DataItem, "W83") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 84">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW84" Checked='<%# DataBinder.Eval(Container.DataItem, "W84") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 85">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW85" Checked='<%# DataBinder.Eval(Container.DataItem, "W85") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 86">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW86" Checked='<%# DataBinder.Eval(Container.DataItem, "W86") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 87">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW87" Checked='<%# DataBinder.Eval(Container.DataItem, "W87") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 88">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW88" Checked='<%# DataBinder.Eval(Container.DataItem, "W88") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 89">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW89" Checked='<%# DataBinder.Eval(Container.DataItem, "W89") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 90">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW90" Checked='<%# DataBinder.Eval(Container.DataItem, "W90") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 91">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW91" Checked='<%# DataBinder.Eval(Container.DataItem, "W91") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 92">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW92" Checked='<%# DataBinder.Eval(Container.DataItem, "W92") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 93">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW93" Checked='<%# DataBinder.Eval(Container.DataItem, "W93") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 94">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW94" Checked='<%# DataBinder.Eval(Container.DataItem, "W94") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 95">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW95" Checked='<%# DataBinder.Eval(Container.DataItem, "W95") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 96">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW96" Checked='<%# DataBinder.Eval(Container.DataItem, "W96") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 97">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW97" Checked='<%# DataBinder.Eval(Container.DataItem, "W97") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 98">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW98" Checked='<%# DataBinder.Eval(Container.DataItem, "W98") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 99">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW99" Checked='<%# DataBinder.Eval(Container.DataItem, "W99") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 100">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW100" Checked='<%# DataBinder.Eval(Container.DataItem, "W100") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 101">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW101" Checked='<%# DataBinder.Eval(Container.DataItem, "W101") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 102">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW102" Checked='<%# DataBinder.Eval(Container.DataItem, "W102") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 103">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW103" Checked='<%# DataBinder.Eval(Container.DataItem, "W103") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Wk 104">
							    <Itemtemplate>
							        <asp:CheckBox Runat="server" ID="cbW104" Checked='<%# DataBinder.Eval(Container.DataItem, "W104") %>' Width="20" AutoPostBack="true" RowIndex="<%# CType(Container,GridViewRow).RowIndex %>" OnCheckedChanged="UpdateWeek"/>
							    </Itemtemplate>
							</asp:TemplateField>
    					</Columns>
        </asp:GridView>
        <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView> 
        <asp:SqlDataSource ID="dsGetSection" runat="server" SelectCommand="SELECT RotationCode, RotationName FROM V_MsRotasiFill" ></asp:SqlDataSource>         
        <br />
        <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
        
          </ContentTemplate>
    </asp:UpdatePanel>               
    </div>
    
    </form>
</body>
</html>
