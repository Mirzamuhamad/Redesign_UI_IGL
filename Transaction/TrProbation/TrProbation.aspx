<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrProbation.aspx.vb" Inherits="Transaction_TrProbation_TrProbation" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
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
	    }catch (err){
            alert(err.description);
          }  
        }       
       function setformat()
        {
        try
         {         
//         var EstCost = document.getElementById("tbEstCost").value.replace(/\$|\,/g,"");                           
//         document.getElementById("tbEstCost").value = setdigit(EstCost,'<%=Viewstate("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
        }
        .style6
        {
            width: 90px;
        }
        .style7
        {
            width: 94px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Probation</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Probation No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Probation Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Employee No</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="Job_Title_Name">Job Title</asp:ListItem>
                    <asp:ListItem Value="AssessmentPeriod">Periode Penilaian</asp:ListItem>
                    <asp:ListItem Value="FgLulus">Lulus</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr1">Approval 1</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr2">Approval 2</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Probation No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Probation Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Employee No</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="Job_Title_Name">Job Title</asp:ListItem>
                    <asp:ListItem Value="AssessmentPeriod">Periode Penilaian</asp:ListItem>
                    <asp:ListItem Value="FgLulus">Lulus</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr1">Approval 1</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr2">Approval 2</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" 
            Visible="False"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" Wrap="false" />
                  <RowStyle CssClass="GridItem" Wrap="false" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
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
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                              </asp:DropDownList>
                              <asp:Button ID="BtnGo" runat="server" class="btngo" 
                                  CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                                  Text="G" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                          HeaderText="Probation No" SortExpression="Nmbr" />
                      <asp:BoundField DataField="Status" HeaderText="Status" 
                          SortExpression="Status" />
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" 
                          HeaderStyle-Width="80px" HeaderText="Probation Date" htmlencode="true" 
                          SortExpression="TransDate" />
                      <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="100px" 
                          HeaderText="Employee No" SortExpression="EmpNumb" />
                      <asp:BoundField DataField="EmpName" HeaderStyle-Width="250px" 
                          HeaderText="Employee Name" SortExpression="EmpName" />
                      <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="150px" 
                          HeaderText="Organization" SortExpression="Dept_Name" />
                      <asp:BoundField DataField="Job_Title_Name" HeaderStyle-Width="150px" 
                          HeaderText="Job Title" SortExpression="Job_Title_Name" />
                      <asp:BoundField DataField="AssessmentPeriod" HeaderStyle-Width="80px" 
                          HeaderText="Periode Penilaian" SortExpression="AssessmentPeriod" />
                      <asp:BoundField DataField="AttendPerc1" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 1(%)" SortExpression="AttendPerc1" />
                      <asp:BoundField DataField="LateMinute1" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 1(Menit)" SortExpression="LateMinute1" />
                      <asp:BoundField DataField="AttendPerc2" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 2(%)" SortExpression="AttendPerc2" />
                      <asp:BoundField DataField="LateMinute2" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 2(Menit)" SortExpression="LateMinute2" />
                      <asp:BoundField DataField="AttendPerc3" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 3(%)" SortExpression="AttendPerc3" />
                      <asp:BoundField DataField="LateMinute3" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 3(Menit)" SortExpression="LateMinute3" />
                      <asp:BoundField DataField="AttendPerc4" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 4(%)" SortExpression="AttendPerc4" />
                      <asp:BoundField DataField="LateMinute4" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 4(Menit)" SortExpression="LateMinute4" />
                      <asp:BoundField DataField="AttendPerc5" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 5(%)" SortExpression="AttendPerc5" />
                      <asp:BoundField DataField="LateMinute5" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 5(Menit)" SortExpression="LateMinute5" />
                      <asp:BoundField DataField="AttendPerc6" HeaderStyle-Width="80px" 
                          HeaderText="Kehadiran 6(%)" SortExpression="AttendPerc6" />
                      <asp:BoundField DataField="LateMinute6" HeaderStyle-Width="80px" 
                          HeaderText="Keterlambatan 6(Menit)" SortExpression="LateMinute6" />
                      <asp:BoundField DataField="FgLulus" HeaderStyle-Width="80px" HeaderText="Lulus" 
                          SortExpression="FgLulus" />
                      <asp:BoundField DataField="KPI" HeaderStyle-Width="80px" HeaderText="KPI" 
                          SortExpression="KPI" />
                      <asp:BoundField DataField="KompetensiUmum" HeaderStyle-Width="80px" 
                          HeaderText="Kompetensi Umum" SortExpression="KompetensiUmum" />
                      <asp:BoundField DataField="KompetensiFungsional" HeaderStyle-Width="80px" 
                          HeaderText="Kompetensi Fungsional" SortExpression="KompetensiFungsional" />
                      <asp:BoundField DataField="Total" HeaderStyle-Width="80px" HeaderText="Total" 
                          SortExpression="Total" />
                      <asp:BoundField DataField="EmpNameAppr1" HeaderStyle-Width="200px" 
                          HeaderText="Emp Appr 1" SortExpression="EmpNameAppr1" />
                      <asp:BoundField DataField="EmpNameAppr2" HeaderStyle-Width="200px" 
                          HeaderText="Emp Appr 2" SortExpression="EmpNameAppr2" />
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                          HeaderText="Remark" SortExpression="Remark" />
                  </Columns>
              </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table style="width: 538px">
        <tr>
            <td class="style6">Probation No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Probation Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
        <tr>
            <td class="style6">Employee</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbEmpNo" runat="server" AutoPostBack="True" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="99px" />
                <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" MaxLength="100" ReadOnly="True" ValidationGroup="Input" Width="262px" />
                <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..." />
            </td>
        </tr>
        <tr>        
            <td class="style6">Organization</td>
            <td>:</td>
            <td colspan="4">
                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" 
                    Enabled="False" Height="17px" ValidationGroup="Input" Width="228px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
              <td class="style6">Job Title</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="DropDownList" 
                      Enabled="False" Height="17px" ValidationGroup="Input" Width="228px">
                  </asp:DropDownList>
              </td>
         </tr>
          <tr>
              <td class="style6">Periode Penilaian</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbPeriod" runat="server" CssClass="TextBox" MaxLength="50" 
                      ValidationGroup="Input" Width="250px" />
              </td>
          </tr>
          <tr>
              <td class="style6">&nbsp;</td>
              <td>&nbsp;</td>
              <td colspan="4">
                  <table>
                      <tr>
                          <td colspan="7">
                              KEDISIPLINAN</td>
                      </tr>
                      <tr style="background-color:Silver;text-align:center">
                          <td>
                              Periode</td>
                          <td>
                              Bulan Ke-1</td>
                          <td>
                              Bulan Ke-2</td>
                          <td>
                              Bulan Ke-3</td>
                          <td>
                              Bulan Ke-4</td>
                          <td>
                              Bulan Ke-5</td>
                          <td>
                              Bulan Ke-6</td>
                      </tr>
                      <tr>
                          <td>
                              Kehadiran (%)</td>
                          <td>
                              <asp:TextBox ID="tbKehadiran1" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbKehadiran2" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbKehadiran3" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbKehadiran4" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbKehadiran5" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbKehadiran6" runat="server" AutoPostBack="True" 
                                  CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="54px" />
                          </td>
                      </tr>
                      <tr>
                          <td>
                              Keterlambatan (Menit)</td>
                          <td>
                              <asp:TextBox ID="tbLate1" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbLate2" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbLate3" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbLate4" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbLate5" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbLate6" runat="server" CssClass="TextBox" MaxLength="50" 
                                  ValidationGroup="Input" Width="54px" />
                          </td>
                      </tr>
                  </table>
              </td>
          </tr>
          <tr>
              <td class="style6">&nbsp;</td>
              <td>&nbsp;</td>
              <td colspan="2">
              <table>
                <tr>
                    <td colspan="3">PENILAIAN KOMPETENSI TEKNIS</td>
                </tr>
                  <tr style="background-color:Silver;text-align:center">
                      <td>No</td>
                      <td>Jenis Kompetnsi</td>
                      <td>Skor</td>
                  </tr>
                <tr>
                    <td>1</td>
                    <td>KPI</td>
                    <td><asp:TextBox ID="tbKPI" runat="server" CssClass="TextBoxR" MaxLength="50" 
                            ValidationGroup="Input" Width="54px" />
                    </td>
                </tr>
                <tr>
                    <td>2</td>
                    <td>Kompetensi Umum</td>
                    <td><asp:TextBox ID="tbUmum" runat="server" CssClass="TextBoxR" MaxLength="50" 
                            ValidationGroup="Input" Width="54px" />
                    </td>
                </tr>
                  <tr>
                      <td>
                          3</td>
                      <td>
                          Kompetensi Fungsional</td>
                      <td>
                          <asp:TextBox ID="tbFungsional" runat="server" CssClass="TextBoxR" 
                              MaxLength="50" ValidationGroup="Input" Width="54px" />
                      </td>
                  </tr>
                  <tr>
                      <td>
                          &nbsp;</td>
                      <td>
                          Total</td>
                      <td>
                          <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" Height="19px" 
                              MaxLength="50" ValidationGroup="Input" Width="54px" />
                      </td>
                  </tr>
              </table>
              </td>
              <td colspan="2">
                  <table>
                      <tr>
                          <td colspan="2">
                              Lulus :
                              <asp:DropDownList ID="ddlLulus" runat="server" CssClass="DropDownList" 
                                  Height="16px" ValidationGroup="Input" Width="45px">
                                  <asp:ListItem>Y</asp:ListItem>
                                  <asp:ListItem>N</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                      </tr>
                      <tr style="background-color:Silver;text-align:center">
                          <td>
                              Skor</td>
                          <td>
                              Ket.</td>
                      </tr>
                      <tr>
                          <td>
                              &nbsp;&nbsp; 0 - 40</td>
                          <td>
                              : PerluPerbaikan</td>
                      </tr>
                      <tr>
                          <td>
                              41 - 60</td>
                          <td>
                              : Dasar</td>
                      </tr>
                      <tr>
                          <td>
                              61 - 80</td>
                          <td>
                              : Baik</td>
                      </tr>
                      <tr>
                          <td>
                              81 - 90</td>
                          <td>
                              : Cakap</td>
                      </tr>
                      <tr>
                          <td>
                              91 - 100</td>
                          <td>
                              : Profesional</td>
                      </tr>
                  </table>
              </td>
          </tr>
          <tr>
              <td class="style6">Approval 1</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbAppr1" runat="server" AutoPostBack="True" CssClass="TextBox" 
                      MaxLength="15" ValidationGroup="Input" Width="100px" />
                  <asp:TextBox ID="tbApprName1" runat="server" CssClass="TextBoxR" 
                      Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                  <asp:Button ID="btnAppr1" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
          </tr>
          <tr>
              <td class="style6">Approval 2</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbAppr2" runat="server" AutoPostBack="True" CssClass="TextBox" 
                      MaxLength="15" ValidationGroup="Input" Width="100px" />
                  <asp:TextBox ID="tbApprName2" runat="server" CssClass="TextBoxR" 
                      Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                  <asp:Button ID="btnAppr2" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
          </tr>
          <tr>
              <td class="style6">
                  Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      MaxLength="50" ValidationGroup="Input" Width="386px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                      <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                                      </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Kegiatan" HeaderText="Kegiatan" />
                            <asp:BoundField DataField="Instruktur" HeaderStyle-Width="100px" HeaderText="Instruktur" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 583px">             
                    <tr>
                        <td class="style7">No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbNo" runat="server" Text="No"></asp:Label></td>
                    </tr>
                    <tr>                    
                        <td class="style7">Kegiatan</td>
                        <td>:</td>
                        <td class="style1"><asp:TextBox ID="tbKegiatan" runat="server" CssClass="TextBox" MaxLength="255" ValidationGroup="Input" Width="445px" />
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Instruktur</td>
                        <td>:</td>
                        <td style="margin-left: 40px"><asp:TextBox ID="tbInstruktur" runat="server" 
                                CssClass="TextBox" Height="18px" MaxLength="50" ValidationGroup="Input" 
                                Width="255px" />
                        </td>
                    </tr>                    
                    <tr>
                        <td class="style7">Remark</td>
                        <td>:</td>
                        <td style="margin-left: 40px"><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" ValidationGroup="Input" Width="445px" />
                        </td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
