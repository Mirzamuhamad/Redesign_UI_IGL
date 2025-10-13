<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCompetenceDept.aspx.vb" Inherits="Transaction_MsCompetenceDept_MsCompetenceDept" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    
    function OpenSchedule(tahun, bulan) {         
            window.open("../../MsCompetenceDept2/MsCompetenceDept2.Aspx?ContainerId=MsCompetenceDept2Id&tahun="+tahun+"&bulan="+bulan,"List","scrollbars=yes,resizable=no,width=700,height=500");        
            return false;
    }   
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style3
        {
            width: 100px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Competence Department</div>
     <hr style="color:Blue" />           
     <br/>  
      <asp:Panel runat="server" ID="PanelHd">
             <fieldset style="width:510px">
                        <table style="width: 515px">
                            <tr>
                                <td class="style3">Department</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDeptCode" MaxLength="10" Width="98px" AutoPostBack = "True"/> 
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDeptName" MaxLength="60" Width="262px"/> 
                                    <asp:Button class="btngo" runat="server" ID="btnDept" Text="..."/> 
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Competence Type</td>
                                <td>:</td>
                                <td><asp:DropDownList ID="ddlComptenceType" runat="server" AutoPostBack="true" CssClass="DropDownList" Height="23px" Width="282px" />
                                </td>
                            </tr>
                            </table>
             </fieldset>
             </asp:Panel>
             <br />
            <br />
       <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>
            </Items>            
        </asp:Menu> <br /> 
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
                 
           <asp:View ID="tab1" runat="server">
              <asp:Panel runat="server" ID="Panel5">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <br/>
             
                    <asp:GridView ID="DataGrid" runat="server" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" Wrap = "false"/>
                        <RowStyle CssClass="GridItem" Wrap="True" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="130" HeaderText="Action" ItemStyle-Wrap = "false">
                                <ItemTemplate>
                                   <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                   <asp:ListItem Selected="True" Text="View" />
                                   <asp:ListItem Text="Edit" />
                                   </asp:DropDownList>
                                   <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
                                </ItemTemplate>
                            <HeaderStyle Width="130" />
                            </asp:TemplateField>
							
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Competence Code" SortExpression="CompetenceCode">
                                <Itemtemplate>
                                    <asp:Label ID="CompetenceCode" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceCode") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:Label ID="CompetenceCodeEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceCode") %>'> </asp:Label>
								</EditItemTemplate>
                                <HeaderStyle Width="80" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="100" HeaderText="Competence Name" SortExpression="CompetenceName">
                                <Itemtemplate>
                                    <asp:Label ID="CompetenceName" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceName") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:Label ID="CompetenceNameEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "CompetemceName") %>'> </asp:Label>
								</EditItemTemplate>
                                <HeaderStyle Width="100" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="15" HeaderText="No" SortExpression="ItemNo">
                                <Itemtemplate>
                                    <asp:Label Width = "15" ID="ItemNo" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Label Width = "15" ID="ItemNoEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'> </asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle Width="15" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="350" HeaderText="Description 1" SortExpression="Description1">
                                <Itemtemplate>
                                    <asp:Label ID="Description1" Width="350" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description1") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="Description1Edit" Width="350" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description1") %>'> </asp:Label> 
                                </EditItemTemplate>
                                <HeaderStyle Width="350" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="350" HeaderText="Description 2" SortExpression="Description2">
                                <Itemtemplate>
                                    <asp:Label ID="Description2" Width="350" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description2") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="Description2Edit" Width="350" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description2") %>'> </asp:Label> 
                                </EditItemTemplate>
                                <HeaderStyle Width="350px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="150" HeaderText="Type" SortExpression="Type">
                                <Itemtemplate>
                                    <asp:Label ID="Type" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="TypeEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'> </asp:Label> 
                                </EditItemTemplate>
                                <HeaderStyle Width="150" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="150" HeaderText="All Job Level" SortExpression="AllJobLevel">
                                <Itemtemplate>
                                    <asp:Label Runat="server" ID="AllJobLevel" text='<%# DataBinder.Eval(Container.DataItem, "AllJobLevel") %>'> </asp:Label>
								</Itemtemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="DropDownList" ID="AllJobLevelEdit" Runat="server" Width="80%" Text='<%# DataBinder.Eval(Container.DataItem, "AllJobLevel") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle Width="150" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="250" HeaderText="Start Range" SortExpression="StartJobLevel">
                                <Itemtemplate>
                                    <asp:Label ID="StartJobLevel" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "StartJobLevel") %>'> </asp:Label> - <asp:Label ID="StartJobLvlName" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "StartJobLvlName") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="StartJobLevelEdit" Runat="server" MaxLength= "5" text='<%# DataBinder.Eval(Container.DataItem, "StartJobLevel") %>'> </asp:TextBox> 
                                </EditItemTemplate>
                                <HeaderStyle Width="250" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="250" HeaderText="End Range" SortExpression="EndJobLevel">
                                <Itemtemplate>
                                    <asp:Label ID="EndJobLevel" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "EndJobLevel") %>'> </asp:Label> - <asp:Label ID="EndJobLvlName" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "EndJobLvlName") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="EndJobLevelEdit" Runat="server" MaxLength= "5" text='<%# DataBinder.Eval(Container.DataItem, "EndJobLevel") %>'> </asp:TextBox> 
                                </EditItemTemplate>
                                <HeaderStyle Width="250" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="80" HeaderText="Priority" SortExpression="Priority">
                                <Itemtemplate>
                                    <asp:Label ID="Priority" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Priority") %>'> </asp:Label> 
                                </Itemtemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="PriorityEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Priority") %>'> </asp:TextBox> 
                                </EditItemTemplate>
                                <HeaderStyle Width="80" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="35" HeaderText="Input" SortExpression="FgInput">
                                <Itemtemplate>
                                    <asp:Label Runat="server" ID="Input" text='<%# DataBinder.Eval(Container.DataItem, "FgInput") %>'> </asp:Label>
								</Itemtemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="DropDownList" ID="InputEdit" Runat="server" Width="80%" Text='<%# DataBinder.Eval(Container.DataItem, "FgInput") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle Width="35" />
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
              </div>   
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlView" Visible="false">
      <table style="width: 639px; height: 313px;">
        <tr>
            <td>Competence Code</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" MaxLength="5" Width="142px"/> &nbsp; &nbsp; </td>            
        </tr>
        <tr>
            <td>Competence Name</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbName" MaxLength="60" Width="420px"/> &nbsp; &nbsp; </td>            
        </tr> 
        <tr>
            <td>No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNo" Width="33px"/></td>            
        </tr>     
        <tr>
            <td>Description 1</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription1" MaxLength="255" Width="418px" Height="64px" TextMode="MultiLine" Enabled = "false"/></td>            
        </tr>
        <tr>
            <td>Description 2</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription2" MaxLength="255" Width="418px" Height="62px" TextMode="MultiLine" Enabled = "false"/></td>            
        </tr>            
        <tr>
            <td>Type</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlType" runat="server" Enabled = "false" CssClass="DropDownList" Height="16px" Width="71px">
                <asp:ListItem>UMUM</asp:ListItem>
                <asp:ListItem>KHUSUS</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
           <td>All Job Level</td>
           <td>:</td>
           <td><asp:DropDownList ID="ddlAllJobLevel" runat="server" CssClass="DropDownList" 
                   Height="16px" Width="36px" AutoPostBack="True" ValidationGroup="Input">
                   <asp:ListItem>Y</asp:ListItem>
                   <asp:ListItem>N</asp:ListItem>
               </asp:DropDownList>
           </td>
          </tr>
          <tr>
            <td>Job Level Start</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlJobLevelStart" runat="server" CssClass="DropDownList" 
                    Enabled="false" Height="16px" Width="312px">
                </asp:DropDownList>
            </td>
          </tr>
          <tr>
             <td>Job Level End</td>
             <td>&nbsp;</td>
             <td>
                 <asp:DropDownList ID="ddlJobLevelEnd" runat="server" CssClass="DropDownList" 
                     Enabled="false" Height="16px" Width="312px">
                 </asp:DropDownList>
             </td>
          </tr>
          <tr>
              <td>Priority</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbPriority" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="62px" />
              </td>
          </tr>
          <tr>
              <td>Input</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlInput" runat="server" CssClass="DropDownList" 
                      Height="16px" ValidationGroup="Input" Width="36px">
                      <asp:ListItem>Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  <br />
                  <asp:Button ID="btnSaveHd" runat="server" class="bitbtndt btnsave" 
                      Text="Save" />
                  <asp:Button ID="btnCancelHd" runat="server" class="bitbtndt btncancel" 
                      CommandName="Cancel" Text="Cancel" />
              </td>
          </tr>
     </table>
      <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Back" />
      <br />
      <br />
      </asp:Panel>  
      </asp:View>   
      </asp:MultiView>
    
      <br />
       
       <asp:SqlDataSource ID="dsAccClass" runat="server" SelectCommand="SELECT DISTINCT Class_Code, Class_Account FROM VMsaccount WHERE FgType = 'PL'">
       </asp:SqlDataSource> 
       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>

</html>
