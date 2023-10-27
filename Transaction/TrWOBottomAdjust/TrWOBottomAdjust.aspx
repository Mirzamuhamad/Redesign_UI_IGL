<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOBottomAdjust.aspx.vb" Inherits="TrWOBottomAdjust" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Adjust Customer Limit</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">         
        
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Wo Bottom Adjust</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Receiving Retur No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="WONo">Wo No</asp:ListItem>
                      <asp:ListItem Value="JObPlant">Job Plant</asp:ListItem>
                      <asp:ListItem Value="WorkBy">Work By Retur No</asp:ListItem>
                      <asp:ListItem Value="Qty">Qty</asp:ListItem>
                      <asp:ListItem Value="Unit">Unit</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                      <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Wo Capacity Adjust : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Receiving Retur No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="WONo">Wo No</asp:ListItem>
                      <asp:ListItem Value="JObPlant">Job Plant</asp:ListItem>
                      <asp:ListItem Value="WorkBy">Work By Retur No</asp:ListItem>
                      <asp:ListItem Value="Qty">Qty</asp:ListItem>
                      <asp:ListItem Value="Unit">Unit</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                      <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
              </asp:DropDownList>              
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	         
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />      
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="No.">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                      HeaderText="Date" SortExpression="TransDate">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="WONo" HeaderText="Wo No" 
                      SortExpression="WONo" />    
                                                                                    
                  <asp:BoundField DataField="WorkBy" HeaderText="Work By" 
                      SortExpression="WorkBy" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="JobPlant" 
                      HeaderText="Job Plan" SortExpression="JobPlant">
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="JobPlantName" 
                      HeaderText="Job Plan Name" SortExpression="JobPlantName">
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="Qty" HeaderText="Qty"
                      SortExpression="Qty" />
                  <asp:BoundField DataField="Unit" HeaderText="Unit" 
                      SortExpression="Unit" />
                                                          
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	 
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Refference</td>
            <td>:</td>
            <td style="width:270px;"><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbTransNo" Width="149px"/> 
            <%--<asp:Label runat="server" Text="   Report : "></asp:Label>
            <asp:DropDownList ID ="ddlReport" Enabled="false" runat ="server" CssClass="DropDownList">
                                    <asp:ListItem>Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>--%> 
            </td>                    
            </tr>                  
             <tr>
                <td>Date</td>
                <td>:</td>
                <td colspan="4">
                    <BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage"  ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />                        
                    </BDP:BasicDatePicker>
                    <asp:Label runat ="server" ID="Label3" ForeColor="Red" Text="*"/>
                </td>                
             </tr>                             
        <tr>
            <td>Wo No</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbWoNo" 
                    CssClass="TextBoxR" Width="150px" ReadOnly="True" />
                <asp:Button Class="btngo" ID="btnWo" Text="..." runat="server"  />                                                                                                                                  
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*" />
            </td> 
            <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>   
        </tr>                                                          
          <tr>
              <td class="style1">
                  Division</td>
              <td class="style1">:</td>
              <td class="style1">
                  <asp:DropDownList ID="ddlDivision" runat="server" Enabled="false" 
                      CssClass="DropDownList" Height="16px" Width="200px"  > 
                  </asp:DropDownList>
                  <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*" />
              </td>
              <td class="style1">
                  &nbsp;</td>
              <td class="style1">&nbsp;</td>
              <td class="style1">
              </td>
          </tr>
          <tr>
              <td>
                  Work By</td>
              <td>:</td>
              <td colspan="4" style="margin-left: 80px">
                  <asp:DropDownList ID="ddlWorkBy" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px"  Width="200px">
                  </asp:DropDownList>
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*" />
             </td>
          </tr>
          <tr>
              <td>
                  Job</td>
              <td>
                  :</td>
              <td colspan="4" style="margin-left: 80px">
                  <asp:DropDownList ID="ddlJob" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                  </asp:DropDownList>
                  <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*" />
              </td>
          </tr>
          <tr>
              <td>
                  Qty</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbQtyHd"  runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="138px" MaxLength = "30" ReadOnly="True" />
                  <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*" />
              </td>
        </tr>    
          <tr>
              <td>
                  Unit</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbUnitHd" runat="server" CssClass="TextBox" MaxLength="30" 
                      ValidationGroup="Input" Width="138px" ReadOnly="True" />
                  <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*" />
              </td>
          </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="369px" MaxLength = "255" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                 
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
   							    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									                            
                            </ItemTemplate>
                            <EditItemTemplate>
                               	<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderStyle-Width="120px" 
                            HeaderText="Type" SortExpression="Type" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DivisiBlok" HeaderText="Code" HeaderStyle-Width="200px" 
                            SortExpression="Divisi_Block" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="BlockName" 
                            HeaderText="Name" SortExpression="BlockName" >                            
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="SplitNmbr" 
                            HeaderText="Split" SortExpression="SplitNmbr" >                            
                            <HeaderStyle Width="40px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="WorkBy" 
                            HeaderText="WorkBy" SortExpression="WorkBy" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Person" 
                            HeaderText="Person" SortExpression="Person" >                            
                            <HeaderStyle Width="40px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Qty" 
                            HeaderText="Qty" SortExpression="Qty" DataFormatString="{0:#,##0.00}" >                            
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="NormaHk" 
                            HeaderText="Capasity" SortExpression="Capasity" >                            
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="WorkDay" 
                            HeaderText="Work Day" SortExpression="WorkDay" >                            
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="StartDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                              HeaderText="Start Date" SortExpression="StartDate">
                              <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="EndDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                              HeaderText="End Date" SortExpression="EndDate">
                              <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                   
                        <asp:BoundField DataField="supplier" HeaderText="Kontraktor" SortExpression="Unit" />                                                
                        <asp:BoundField DataField="supplierName" HeaderStyle-Width="80px" 
                            HeaderText="Kontraktor Name" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	                 
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>
                        <a href="javascript:__doPostBack('lbProduct','')">Type</a></td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbType" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" />
                        <asp:TextBox ID="tbTypeName" runat="server" CssClass="TextBox" Visible="false"  Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="200px" />
                        <asp:Button Class="btngo" ID="btnDt" Text="..." runat="server"  /> 
                    </td>
                </tr>
                <tr>
                    <td>
                        Code</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="tbDivisiCode" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" Width="61px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Name :&nbsp;
                        <asp:TextBox ID="tbDivisiName" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" Width="61px" />
                     </td>
                </tr>
                <tr>
                    <td>
                        Split</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:TextBox ID="tbSplit" type="number" runat="server" AutoPostBack="False" CssClass="TextBox" 
                            Width="45px" Enabled ="False" /> 
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>                                
                <tr>
                    <td>
                        Work By</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbWorkBy" Enabled ="False" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Person</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbPerson" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" Enabled ="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Capacity</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbCapacity" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" Enabled ="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Work Day</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbWorkDay" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" Enabled ="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartDate" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; End Date :
                        <BDP:BasicDatePicker ID="tbEndDate" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        Kontraktor</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbKontraktor" runat="server" AutoPostBack="False" 
                            CssClass="TextBox" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Fg Borongan :&nbsp;&nbsp;
                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFg">
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem  >N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />	
            <br />
       </asp:Panel> 
       <br />          
    	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" 
            Text="Save" validationgroup="Input" Height="18px" Width="64px"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	  
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
