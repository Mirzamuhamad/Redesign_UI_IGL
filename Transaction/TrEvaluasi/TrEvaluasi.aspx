<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrEvaluasi.aspx.vb" Inherits="Transaction_TrEvaluasi_TrEvaluasi" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
       


        function hitungtotal(){   
          try
          { 
            var _tnilai1 = parseFloat(document.getElementById("tbNilai1").value.replace(/\$|\,/g,""));
            var _tnilai2 = parseFloat(document.getElementById("tbNilai2").value.replace(/\$|\,/g,""));
            var _tnilai3 = parseFloat(document.getElementById("tbNilai3").value.replace(/\$|\,/g,""));
            var _tnilai4 = parseFloat(document.getElementById("tbNilai4").value.replace(/\$|\,/g,""));
            var _tnilai5 = parseFloat(document.getElementById("tbNilai5").value.replace(/\$|\,/g,""));
                                             
            var _tbobot1 = parseFloat(document.getElementById("tbBobot1").value.replace(/\$|\,/g,""));
            var _tbobot2 = parseFloat(document.getElementById("tbBobot2").value.replace(/\$|\,/g,""));
            var _tbobot3 = parseFloat(document.getElementById("tbBobot3").value.replace(/\$|\,/g,""));
            var _tbobot4 = parseFloat(document.getElementById("tbBobot4").value.replace(/\$|\,/g,""));
            var _tbobot5 = parseFloat(document.getElementById("tbBobot5").value.replace(/\$|\,/g,""));   
            
            var _tnilaiakhir1 = parseFloat(document.getElementById("tbNilaiAkhir1").value.replace(/\$|\,/g,""));
            var _tnilaiakhir2 = parseFloat(document.getElementById("tbNilaiAkhir2").value.replace(/\$|\,/g,""));
            var _tnilaiakhir3 = parseFloat(document.getElementById("tbNilaiAkhir3").value.replace(/\$|\,/g,""));
            var _tnilaiakhir4 = parseFloat(document.getElementById("tbNilaiAkhir4").value.replace(/\$|\,/g,""));
            var _tnilaiakhir5 = parseFloat(document.getElementById("tbNilaiAkhir5").value.replace(/\$|\,/g,""));
                        
            _tnilaiakhir1 = ( _tnilai1 * _tbobot1 ) / 100          
            _tnilaiakhir2 = ( _tnilai2 * _tbobot2 ) / 100          
            _tnilaiakhir3 = ( _tnilai3 * _tbobot3 ) / 100          
            _tnilaiakhir4 = ( _tnilai4 * _tbobot4 ) / 100          
                        
            _tnilai5 = _tnilai1 + _tnilai2 + _tnilai3 + _tnilai4;
            _tbobot5 = _tbobot1 + _tbobot2 + _tbobot3 + _tbobot4;
            _tnilaiakhir5 = _tnilaiakhir1 + _tnilaiakhir2 + _tnilaiakhir3 + _tnilaiakhir4;
            
            document.getElementById("tbNilai1").value = setdigit(_tnilai1, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilai2").value = setdigit(_tnilai2, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilai3").value = setdigit(_tnilai3, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilai4").value = setdigit(_tnilai4, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilai5").value = setdigit(_tnilai5, '<%=VIEWSTATE("DigitQty")%>');
            
            document.getElementById("tbBobot1").value = setdigit(_tbobot1, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbBobot2").value = setdigit(_tbobot2, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbBobot3").value = setdigit(_tbobot3, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbBobot4").value = setdigit(_tbobot4, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbBobot5").value = setdigit(_tbobot5, '<%=VIEWSTATE("DigitQty")%>');
                        
            document.getElementById("tbNilaiAkhir1").value = setdigit(_tnilaiakhir1, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilaiAkhir2").value = setdigit(_tnilaiakhir2, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilaiAkhir3").value = setdigit(_tnilaiakhir3, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilaiAkhir4").value = setdigit(_tnilaiakhir4, '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNilaiAkhir5").value = setdigit(_tnilaiakhir5, '<%=VIEWSTATE("DigitQty")%>');
          
          }catch (err){
            alert(err.description);
          }
        }
        
   
        
   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            width: 94px;
        }
        .style2
        {
            width: 8px;
        }
        .style3
        {
            width: 100px;
        }
        .style4
        {
            width: 13px;
        }
        .style5
        {
            width: 9px;
        }
        .style6
        {
            width: 20px;
        }
        .style7
        {
            width: 50px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Evaluasi Performa</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Evaluasi No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Evaluasi Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="CompetenceType">Type</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Emp No</asp:ListItem>                                        
                    <asp:ListItem Value="Emp_Name">Emp Name</asp:ListItem>                    
                    <asp:ListItem Value="Job_Title_Name">Job Title</asp:ListItem>
                    <asp:ListItem Value="Job_Level_Name">Job Level</asp:ListItem>
                    <asp:ListItem Value="Emp_NameAppr1">Nama Penilai</asp:ListItem>
                    <asp:ListItem Value="Job_TitleAppr1">Job Title Penilai</asp:ListItem>
                    <asp:ListItem Value="Job_levelAppr1">Job Level Penilai</asp:ListItem>
                    <asp:ListItem Value="ReviewPeriod">Review Period</asp:ListItem>
                    <asp:ListItem Value="ReviewReason">Review Reason</asp:ListItem>
                    <asp:ListItem Value="Kekuatan">Kekuatan</asp:ListItem>
                    <asp:ListItem Value="Improvement">Improvement</asp:ListItem>
                    <asp:ListItem Value="Pencapaian">Pencapaian</asp:ListItem>
                    <asp:ListItem Value="KomentarEmpAppr1">KomentarEmpAppr1</asp:ListItem>
                    <asp:ListItem Value="KomentarEmpAppr2">KomentarEmpAppr2</asp:ListItem>                    
                  </asp:DropDownList>
                                   
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Evaluasi No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Evaluasi Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="CompetenceType">Type</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Emp No</asp:ListItem>                                        
                    <asp:ListItem Value="Emp_Name">Emp Name</asp:ListItem>                    
                    <asp:ListItem Value="Job_Title_Name">Job Title</asp:ListItem>
                    <asp:ListItem Value="Job_Level_Name">Job Level</asp:ListItem>
                    <asp:ListItem Value="Emp_NameAppr1">Nama Penilai</asp:ListItem>
                    <asp:ListItem Value="Job_TitleAppr1">Job Title Penilai</asp:ListItem>
                    <asp:ListItem Value="Job_levelAppr1">Job Level Penilai</asp:ListItem>
                    <asp:ListItem Value="ReviewPeriod">Review Period</asp:ListItem>
                    <asp:ListItem Value="ReviewReason">Review Reason</asp:ListItem>
                    <asp:ListItem Value="Kekuatan">Kekuatan</asp:ListItem>
                    <asp:ListItem Value="Improvement">Improvement</asp:ListItem>
                    <asp:ListItem Value="Pencapaian">Pencapaian</asp:ListItem>
                    <asp:ListItem Value="KomentarEmpAppr1">Komentar Penilai</asp:ListItem>
                    <asp:ListItem Value="KomentarEmp">Komentar Karyawan</asp:ListItem>
                    <asp:ListItem Value="KomentarEmpAppr2">Komentar Manager</asp:ListItem>                                
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
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Candidate No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Candidate Date"></asp:BoundField>
                  <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="80px" SortExpression="Dept_Name" HeaderText="Organization Name"></asp:BoundField>
                  <asp:BoundField DataField="CompetenceType" HeaderStyle-Width="80px" SortExpression="CompetenceType" HeaderText="Competence Type"></asp:BoundField>
                  <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="200px" SortExpression="EmpNumb" HeaderText="Emp No"></asp:BoundField>
                  <asp:BoundField DataField="Emp_Name" HeaderStyle-Width="80px" SortExpression="Emp_Name" HeaderText="Emp Name"></asp:BoundField>
                  <asp:BoundField DataField="Job_Title_Name" HeaderStyle-Width="200px" SortExpression="Job_Title_Name" HeaderText="Job Title"></asp:BoundField>
                  <asp:BoundField DataField="Job_Level_Name" HeaderStyle-Width="80px" SortExpression="Job_Level_Name" HeaderText="Job Level"></asp:BoundField>
                  <asp:BoundField DataField="Emp_NameAppr1" HeaderStyle-Width="250px" SortExpression="Emp_NameAppr1" HeaderText="Nama Penilai"></asp:BoundField>
                  <asp:BoundField DataField="Job_TitleAppr1" HeaderStyle-Width="80px" SortExpression="Job_TitleAppr1" HeaderText="Job Title Penilai"></asp:BoundField>
                  <asp:BoundField DataField="Job_levelAppr1" HeaderStyle-Width="250px" SortExpression="Job_levelAppr1" HeaderText="Job Level Penilai"></asp:BoundField>
                  <asp:BoundField DataField="Emp_NameAppr2" HeaderStyle-Width="250px" SortExpression="Emp_NameAppr2" HeaderText="Nama Atasan Penilai"></asp:BoundField>
                  <asp:BoundField DataField="Job_TitleAppr2" HeaderStyle-Width="250px" SortExpression="Job_TitleAppr2" HeaderText="Job Title Atasan Penilai"></asp:BoundField>
                  <asp:BoundField DataField="Job_LevelAppr2" HeaderStyle-Width="250px" SortExpression="Job_LevelAppr2" HeaderText="Job Level Atasan Penilai"></asp:BoundField>
                  <asp:BoundField DataField="ReviewPeriod" HeaderStyle-Width="80px" SortExpression="ReviewPeriod" HeaderText="Review Period"></asp:BoundField>
                  <asp:BoundField DataField="ReviewReason" HeaderStyle-Width="250px" SortExpression="ReviewReason" HeaderText="Review Reason"></asp:BoundField>
                  <asp:BoundField DataField="Kekuatan" HeaderStyle-Width="80px" SortExpression="Kekuatan" HeaderText="Kekuatan"></asp:BoundField>
                  <asp:BoundField DataField="Improvement" HeaderStyle-Width="250px" SortExpression="Improvement" HeaderText="Improvement"></asp:BoundField>
                  <asp:BoundField DataField="Pencapaian" HeaderStyle-Width="80px" SortExpression="Pencapaian" HeaderText="Pencapaian"></asp:BoundField>
                  <asp:BoundField DataField="FgSetuju" HeaderStyle-Width="250px" SortExpression="FgSetuju" HeaderText="Fg Setuju"></asp:BoundField>
                  <asp:BoundField DataField="KomentarEmpAppr1" HeaderStyle-Width="80px" SortExpression="KomentarEmpAppr1" HeaderText="Komentar Penilai"></asp:BoundField>
                  <asp:BoundField DataField="KomentarEmp" HeaderStyle-Width="250px" SortExpression="KomentarEmp" HeaderText="Komentar Karyawan"></asp:BoundField>
                  <asp:BoundField DataField="KomentarEmpAppr2" HeaderStyle-Width="80px" SortExpression="KomentarEmpAppr2" HeaderText="Komentar Manager"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                  
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
            <td>Evaluasi No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Evaluasi Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>
        <tr>
            <td>Organization</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbDeptCode" MaxLength="15" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbDeptName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnDept" Text="..." runat="server" ValidationGroup="Input" />
            </td>            
        </tr> 
        <tr>
            <td>Type</td>
             <td>:</td>
             <td>
                 <asp:DropDownList CssClass="DropDownList" ID="ddlCompType" Width="125px" ValidationGroup="Input" runat="server" AutoPostBack="true" />
             </td>    
        </tr>
        <tr>
            <td>Employee</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmp" MaxLength="15" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbEmpName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnEmp" Text="..." runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Job Title</td>
             <td>:</td>
             <td> <asp:DropDownList CssClass="DropDownList" ID="ddlJobTitle" runat="server" Width="225px" Enabled ="false"/></td> 
              <td>Job Level</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" ID="ddlJobLevel" runat="server" Width="225px" Enabled ="false"/></td>      
        </tr>
       
       <tr>
            <td>Periode Review</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbReviewPeriod" MaxLength="100" CssClass="TextBox" Width="225px"/></td>
            <td>Reason</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" ID="ddlReviewReason" runat="server" Width="225px" ValidationGroup="Input" /></td>                        
        </tr>
               
        <tr>
            <td>Penilai</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmpAppr1" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbEmpNameAppr1" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnEmpAppr1" Text="..." runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Job Title Penilai</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" ID="ddlJobTitleAppr1" runat="server" Width="225px" Enabled ="false"/></td>  
             <td>Job Level Penilai</td>
             <td>:</td>
             <td> <asp:DropDownList CssClass="DropDownList" ID="ddlJobLevelAppr1" runat="server" Width="225px" Enabled ="false"/></td>   
        </tr>
       
        <tr>
            <td>Atasan Penilai</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmpAppr2" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbEmpNameAppr2" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnEmpAppr2" Text="..." runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Job Title Atasan Penilai</td>
             <td>:</td>
             <td> <asp:DropDownList CssClass="DropDownList" ID="ddlJobTitleAppr2" runat="server" Width="225px" Enabled ="false"/></td>  
             <td>Job Level Atasan Penilai</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" ID="ddlJobLevelAppr2" runat="server" Width="225px" Enabled ="false"/></td>      
        </tr>
       
      </table>  
      
      <br />      
      
       <asp:Menu ID="Menu1" runat="server" CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Ringkasan" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Penilaian Akhir" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Komentar" Value="2"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
               
              <asp:Panel runat="server" ID="pnlRingkasan">
                <table> 
                  
                    <tr>
                        <td>Kekuatan</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbKekuatan" MaxLength="255" Width="300px"/>
                        </td> 
                        
                    </tr> 
                    <tr>                        
                        <td>Area Perbaikan</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbImprovement" MaxLength="255" Width="300px"/>
                        </td>
                    </tr>            
                    <tr>                        
                        <td>Kehadiran dan atau SP</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbPencapaian" MaxLength="255" Width="300px"/>
                        </td>
                    </tr>  
                             
                </table>
                								
           </asp:Panel> 
              
           </asp:View>           
           <asp:View ID="Tab2" runat="server">
                
              <asp:Panel runat="server" ID="pnlNilai" >
              <table style="width: 538px">
              <tr>              
              <td colspan="2">
                <table>    
                    <tr  style="background-color:Silver;text-align:center">
                        <td>Rating Summary<td>          
                        
                        <td>Nilai</td>
                        <td>Bobot</td>
                        <td>Nilai Akhir</td>
                    </tr>
                    <tr>                    
                        <td>KPI</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilai1" Width="50px" MaxLength="60" AutoPostBack="true" /></td>  
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBobot1" Width="50px" MaxLength="60" AutoPostBack="true" /></td>  
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilaiAkhir1" Width="50px" MaxLength="60" AutoPostBack="true" /></td>  
                    </tr>   
               
                    <tr>
                        <td>Kompetensi Umum</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilai2" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBobot2" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilaiAkhir2" Width="50px" MaxLength="60" /></td>  
                    </tr>
                
                     <tr>
                        <td>Kompetensi Fungsional + Supervisor</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilai3" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBobot3" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilaiAkhir3" Width="50px" MaxLength="60" /></td>  
                     </tr>  
                     
                      <tr>
                        <td>Project</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilai4" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbBobot4" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilaiAkhir4" Width="50px" MaxLength="60" /></td>  
                     </tr> 
                                
                      <tr>
                        <td>Total Nilai Keseluruhan</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilai5" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBobot5" Width="50px"  MaxLength="60" /></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNilaiAkhir5" Width="50px" MaxLength="60" /></td>  
                     </tr>                        
                </table>
                </td>
                <td class="style6">&nbsp;</td>
                <td colspan="2">
                <table>                      
                      <tr style="background-color:Silver;text-align:center">
                          <td class="style7">Skor</td>
                          <td>Ket.</td>
                      </tr>
                      <tr>
                          <td>0 - 40</td>
                          <td>: PerluPerbaikan</td>
                      </tr>
                      <tr>
                          <td>41 - 60</td>
                          <td>: Dasar</td>
                      </tr>
                      <tr>
                          <td>61 - 80</td>
                          <td>: Baik</td>
                      </tr>
                      <tr>
                          <td>81 - 90</td>
                          <td>: Cakap</td>
                      </tr>
                      <tr>
                          <td>91 - 100</td>
                          <td>: Profesional</td>
                      </tr>
                  </table>
                  </td>
                  </tr>
                  </table>
           </asp:Panel> 
               
            </asp:View>    
           <asp:View ID="Tab3" runat="server">
                
              <asp:Panel runat="server" ID="pnlKomentar">
                <table> 
                     
                     <tr>
                        Atasan dan Saya telah bertemu dan berdiskusi tentang performa saya
                        <td>Saya</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgSetuju" runat="server" >
                                    <asp:ListItem value="Y" Selected="True">Setuju</asp:ListItem>
                                    <asp:ListItem value="N">Tidak Setuju</asp:ListItem>
                                </asp:DropDownList> 
                                dengan penilaian ini
                        </td>             
                     </tr>
                     
                     <tr>
                        <td>Penilai</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbKomentarEmpAppr1" MaxLength="255" Width="300px"/>
                        </td> 
                        
                    </tr> 
                    <tr>                        
                        <td>Karyawan</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbKomentarEmp" MaxLength="255" Width="300px"/>
                        </td>
                    </tr>            
                    <tr>                        
                        <td>Atasan lebih tinggi / Manager</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbKomentarEmpAppr2" MaxLength="255" Width="300px"/>
                        </td>
                    </tr>    
                    
                </table>
               

           </asp:Panel> 
               
            </asp:View>        
        </asp:MultiView>
    
       <br /> 
       <br />      
     <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu2"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Goal KPI" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Kompetensi" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Project" Value="2"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
           <asp:View ID="View1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" Visible="false" ValidationGroup="Input" />	
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
   							          <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								      <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                      </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Indikator" HeaderStyle-Width="150px" HeaderText="Indikator" />
                            <asp:BoundField DataField="Bobot" HeaderStyle-Width="80px" HeaderText="Bobot" />
                            <asp:BoundField DataField="Realisasi" HeaderStyle-Width="80px" HeaderText="Realisasi" />
                            <asp:BoundField DataField="Target" HeaderStyle-Width="80px" HeaderText="Target" />                            
                            <asp:BoundField DataField="Type" HeaderStyle-Width="80px" HeaderText="Type" />
                            <asp:BoundField DataField="Skor" HeaderStyle-Width="80px" HeaderText="Skor" />
                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" Visible="false" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table> 
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNoDt" runat="server" Text="itemmm" />
                        </td>           
                    </tr> 
                    <tr>                        
                        <td>Indikator</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbIndikator" MaxLength="255" Width="250px"/>                                                       
                        </td>
                    </tr>             
                    <tr>                    
                        <td></td>
                        <td></td>
                        <td><table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Bobot</td>
                                <td>Target</td>
                                <td>MIN/MAX</td>
                                <td>Realisasi</td>
                                <td>Skor</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="tbBobotDt" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBox" AutoPostBack="true"/></td>
                                <td><asp:TextBox ID="tbTarget" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBox" AutoPostBack="true"/></td>
                                <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlTypeDt" Width="60px" runat="server" AutoPostBack="true">
                                        <asp:ListItem Selected="True">MIN</asp:ListItem>
                                        <asp:ListItem >MAX</asp:ListItem>
                                    </asp:DropDownList> 
                                </td>  
                                <td><asp:TextBox ID="tbRealisasi" ValidationGroup="Input" runat="server" Width="80px"  CssClass="TextBox" AutoPostBack="true"/></td>
                                <td><asp:TextBox ID="tbSkor" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR" Enabled="false"/></td>
                            </tr>
                            </table>
                          </td>  
                    </tr>    
                     
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
           <asp:View ID="View2" runat="server">
              <asp:Panel runat="server" ID="pnlDt2">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnGetKompetensi" Text="Get Kompetensi" Width="110px" Visible="false" ValidationGroup="Input" />	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
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
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Type" HeaderStyle-Width="100px" HeaderText="Type" />
                            <asp:BoundField DataField="CompetenceCode" HeaderStyle-Width="100px" HeaderText="Competence Code" />
                            <asp:BoundField DataField="CompetenceName" HeaderStyle-Width="150px" HeaderText="Competence Name" />
                            <asp:BoundField DataField="CompetenceItem" HeaderStyle-Width="50px" HeaderText="Item No" />
                            <asp:BoundField DataField="Description1" HeaderStyle-Width="150px" HeaderText="Description 1" />
                            <asp:BoundField DataField="Description2" HeaderStyle-Width="150px" HeaderText="Description 2" />
                            <asp:BoundField DataField="Nilai" HeaderStyle-Width="100px" HeaderText="Nilai" />                            
                                                        
                        </Columns>
                    </asp:GridView>
              </div>   
              
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table> 
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNoDt2" runat="server" Text="itemmm" />
                        </td>           
                    </tr> 
                    <tr>                        
                        <td>Type</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbTypeDt2" Enabled="false" MaxLength="255" Width="50px"/></td>
                        
                    </tr>   
                    <tr>                        
                        <td>Competence</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbCompCode" Enabled="false" MaxLength="60" Width="225px"/>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbCompName" Enabled="false" MaxLength="60" Width="225px"/>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbCompNo" Enabled="false" MaxLength="60" Width="225px"/>
                        </td>
                    </tr>   
                    <tr>                        
                        <td>Description 1</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription1" Enabled="false" MaxLength="255" Width="250px"/></td>
                    </tr>  
                    <tr>                        
                        <td>Description 2</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription2" Enabled="false" MaxLength="255" Width="250px"/></td>
                    </tr>                     
                    <tr>                        
                        <td>Nilai</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbNilaiDt2" MaxLength="255" Width="50px"/></td>
                        
                    </tr>       
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>   
           <asp:View ID="View3" runat="server">
              <asp:Panel runat="server" ID="pnlDt3">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" Visible="false" ValidationGroup="Input" />	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="false" 
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
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="ProjectName" HeaderStyle-Width="250px" HeaderText="Project Name" />
                            <asp:BoundField DataField="Pencapaian" HeaderStyle-Width="250px" HeaderText="Pencapaian" />
                            <asp:BoundField DataField="Nilai" HeaderStyle-Width="80px" HeaderText="Nilai" />
                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" Visible="false" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                <table> 
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNoDt3" runat="server" Text="itemmm" />
                        </td>           
                    </tr> 
                    <tr>                        
                        <td>Projects</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbProject" MaxLength="255" Width="250px"/>                                                       
                        </td>
                    </tr>   
                    <tr>                        
                        <td>Pencapaian</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbPencapaianDt3" MaxLength="255" Width="250px"/>                                                       
                        </td>
                    </tr>   
                    <tr>                        
                        <td>Nilai</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbNilaiDt3" Width="50px"/>                                                       
                        </td>
                    </tr>   
                          
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>  
        </asp:MultiView>
         
       <br />         
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
