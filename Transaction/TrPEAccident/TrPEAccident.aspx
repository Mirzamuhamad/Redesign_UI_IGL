<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEAccident.aspx.vb" Inherits="Transaction_TrPEAccident_TrPEAccident" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Laporan Kecelakaan Kerja</title>
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
    
        function setformat()
        {
        try
         {         
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
//        var QtyRR = document.getElementById("tbQtyRR").value.replace(/\$|\,/g,""); 
        var QtyOrder = document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""); 
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
//        document.getElementById("tbQtyRR").value = setdigit(QtyRR,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOrder").value = setdigit(QtyOrder,'<%=ViewState("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Laporan Kecelakaan Kerja</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Transnmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Accidenttype">Accident Type</asp:ListItem>
                      <asp:ListItem Value="Emp_Code">Employee Code</asp:ListItem>
                      <asp:ListItem Value="Emp_Name">Employee Name</asp:ListItem>
                      <asp:ListItem Value="Department">Organization</asp:ListItem>
                      <asp:ListItem Value="DepartmentName">Organization Name</asp:ListItem>
                      <asp:ListItem Value="Job_Title_Code">Job Title Code</asp:ListItem>
                      <asp:ListItem Value="Job_Title_Name">Job Title Name</asp:ListItem>
                      <asp:ListItem Value="Gender">Gender</asp:ListItem>                      
                      <asp:ListItem Value="Emp_Status_Code">Status</asp:ListItem>
                      <asp:ListItem Value="Emp_Status_Name">Status Name</asp:ListItem>                 
                      <asp:ListItem Value="MasaKerja">Masa Kerja</asp:ListItem>                 
                      <asp:ListItem Value="Accident_Time">Accident Time</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentDamage">Accident Damage</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentPlace">Accident Place</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentWhen">Moment Of Accident</asp:ListItem>                                  
                      <asp:ListItem Value="Perawatan">Injury Treatment</asp:ListItem>           
                      <asp:ListItem Value="Hospital">Hospital</asp:ListItem>                         
                      <asp:ListItem Value="TempatLuka">Part of Body Injured</asp:ListItem>                                  
                      <asp:ListItem Value= "AccidentUraian">Accident Description</asp:ListItem>                                  
                      <asp:ListItem Value= "NamaSaksi">Witness Name</asp:ListItem>                                  
                      <asp:ListItem Value= "DepartmentSaksi">Witness Organization</asp:ListItem>                                  
                      <asp:ListItem Value= "KondisiTakAman">Unsafe Conditions</asp:ListItem>                                  
                      <asp:ListItem Value= "TindakanTakAman">Unsafe Acts</asp:ListItem>                                  
                      <asp:ListItem Value= "SaranManager">Manager Advice</asp:ListItem>                                  
                      <asp:ListItem Value= "SaranSafety">Safety Advice</asp:ListItem>                                  
                      <asp:ListItem Value= "Remark">Remark</asp:ListItem>                              
                      
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
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
                      <asp:ListItem Value="Transnmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Accidenttype">Accident Type</asp:ListItem>
                      <asp:ListItem Value="Emp_Code">Employee Code</asp:ListItem>
                      <asp:ListItem Value="Emp_Name">Employee Name</asp:ListItem>
                      <asp:ListItem Value="Department">Organization</asp:ListItem>
                      <asp:ListItem Value="DepartmentName">Organization Name</asp:ListItem>
                      <asp:ListItem Value="Job_Title_Code">Job Title Code</asp:ListItem>
                      <asp:ListItem Value="Job_Title_Name">Job Title Name</asp:ListItem>
                      <asp:ListItem Value="Gender">Gender</asp:ListItem>                      
                      <asp:ListItem Value="Emp_Status_Code">Status</asp:ListItem>
                      <asp:ListItem Value="Emp_Status_Name">Status Name</asp:ListItem>                 
                      <asp:ListItem Value="MasaKerja">Work Period</asp:ListItem>                 
                      <asp:ListItem Value="Accident_Time">Accident Time</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentDamage">Accident Damage</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentPlace">Accident Place</asp:ListItem>                                  
                      <asp:ListItem Value="AccidentWhen">Moment of Accident</asp:ListItem>                                  
                      <asp:ListItem Value="Perawatan">Injury Treatment</asp:ListItem>                                  
                      <asp:ListItem Value="Hospital">Hospital</asp:ListItem>                                  
                      <asp:ListItem Value="TempatLuka">Part Of Body Injured</asp:ListItem>                                  
                      <asp:ListItem Value= "AccidentUraian">Accident Description</asp:ListItem>                                  
                      <asp:ListItem Value= "NamaSaksi">Witness Name</asp:ListItem>                                  
                      <asp:ListItem Value= "DepartmentSaksi">Witness Organization</asp:ListItem>                                  
                      <asp:ListItem Value= "KondisiTakAman">Unsafe Conditions</asp:ListItem>                                  
                      <asp:ListItem Value= "TindakanTakAman">Unsafe Acts</asp:ListItem>                                  
                      <asp:ListItem Value= "SaranManager">Manager Advice</asp:ListItem>                                  
                      <asp:ListItem Value= "SaranSafety">Safety Advice</asp:ListItem>                                  
                      <asp:ListItem Value= "Remark">Remark</asp:ListItem>                            
                      
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
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" />
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
                              <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="Transnmbr" HeaderStyle-Width="120px" 
                          HeaderText="Reference" SortExpression="Nmbr">
                          <HeaderStyle Width="120px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Status" HeaderText="Status" />
                      <asp:BoundField DataField="transdate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                          HeaderText="Date" SortExpression="transdate">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Accidenttype" HeaderText="Accident Type" 
                          SortExpression="Accidenttype" />
                      <asp:BoundField DataField="Emp_Code" HeaderText="Employee Code" 
                          SortExpression="Emp_Code" />                      
                      <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" SortExpression="Emp_Name" />                      
                      <asp:BoundField DataField="Department" HeaderStyle-Width="102px" 
                          HeaderText="Organization" SortExpression="Department">
                          <HeaderStyle Width="200px" />                       
                     </asp:BoundField>                                                  
                     <asp:BoundField DataField="DepartmentName" HeaderStyle-Width="102px" 
                          HeaderText="Organization Name" SortExpression="OrganizationName">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>                         
                      <asp:BoundField DataField="Job_Title_Code" HeaderText="Job Title Code" SortExpression="Job_Title_Code" />
                      <asp:BoundField DataField="Job_Title_Name" HeaderStyle-Width="102px" 
                          HeaderText="Job Title Name" SortExpression="Job_Title_Name">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>              
                       <asp:BoundField DataField="Gender" HeaderText="Gender" 
                          SortExpression="Gender" />             
                      <asp:BoundField DataField="Emp_Status_Code" HeaderText="Status" SortExpression="Emp_Status_Code" />
                      <asp:BoundField DataField="Emp_Status_Name" HeaderText="Status Name" SortExpression="Emp_Status_Name" />                                            
                      <asp:BoundField DataField="MasaKerja" HeaderText="Work Period" SortExpression="MasaKerja" />                                                                 
                      <asp:BoundField DataField="Accident_Time" HeaderText="Accident Time" SortExpression="Accident_Time" />      
                      <asp:BoundField DataField="AccidentPlace" HeaderStyle-Width="102px" 
                          HeaderText="Accident Place" SortExpression="AccidentPlace">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>                                      
                      <asp:BoundField DataField="AccidentDamage" HeaderText="Accident Damage" SortExpression="AccidentDamage" />                      
                      <asp:BoundField DataField="AccidentWhen" HeaderText="Moment Of Accident" SortExpression="AccidentWhen" />                     
                      <asp:BoundField DataField="Perawatan" HeaderText="Injury Treatment" SortExpression="Perawatan"/>
                      <asp:BoundField DataField="Hospital" HeaderStyle-Width="200px" 
                          HeaderText="Hospital" SortExpression = "Hospital">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="TempatLuka" HeaderText="Part Of Body Injured" SortExpression="TempatLuka"/>
                      <asp:BoundField DataField="AccidentUraian" HeaderText="Accident Description" SortExpression="AccidentUraian"/>
                      <asp:BoundField DataField="NamaSaksi" HeaderText="Witness Name" SortExpression="NamaSaksi"/>
                      <asp:BoundField DataField="DepartmentSaksi" HeaderText="Witness Organization" SortExpression="DepartmentSaksi"/>             
                      <asp:BoundField DataField="KondisiTakAman" HeaderText="Unsafe Conditions" SortExpression="KondisiTakAman"/>
                      <asp:BoundField DataField="TindakanTakAman" HeaderText="Unsafe Acts" SortExpression="TindakanTakAman"/>
                      <asp:BoundField DataField="SaranManager" HeaderText="Manager Advice" SortExpression="SaranManager"/>
                      <asp:BoundField DataField="SaranSafety" HeaderText="Safety Advice" SortExpression="SaranSafety"/>                                   
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                          HeaderText="Remark" SortExpression = "Remark">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                  </Columns>
              </asp:GridView>
          </div>
          
            <asp:Panel runat="server" ID ="pnlNav" Visible="False">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	 
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>                    
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
         <tr>
             <td>Accident Type</td>
             <td>:</td>
             <td >
                  <asp:DropDownList ID="ddlTipe"  CssClass="DropDownList" ValidationGroup="Input" runat="server" 
                       Width="100px">
                      <asp:ListItem >IN PLANT</asp:ListItem>
                      <asp:ListItem >OUT PLANT</asp:ListItem>
                  </asp:DropDownList> 
                  <asp:TextBox runat="server" ID="TextBox2" Visible="false"/>                
              </td>
              <td>
                  Accident Time</td>
              <td>
                    :</td>
              <td>
                    <asp:TextBox ID="tbAccidentTime" runat="server" CssClass="TextBox" MaxLength="2" 
                          ValidationGroup="Input" Width="15px" />
                     <asp:TextBox ID="tbAccidentTime2" runat="server" CssClass="TextBox" MaxLength="2" 
                          ValidationGroup="Input"  Width="15px" />
              </td>
          </tr>      
          
          <tr>
           <td>
               <asp:LinkButton ID="lbEmployee" ValidationGroup="Input" runat="server" Text="Employee"/>
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbEmployee" runat="server" AutoPostBack="true" MaxLength = "15" 
                    CssClass="TextBox" />
                <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="200px" />
                <asp:Button Class="btngo" ID="btnEmployee" Text="..." runat="server" ValidationGroup="Input" />                                                       
            </td> 
          </tr>    
          
          <tr>
            <td><asp:LinkButton ID="lbDepartment" ValidationGroup="Input" runat="server" Text="Organization"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlDepartment" enabled = "False"  Width="200px" />                
                <asp:TextBox runat="server" ID="TextBox3" Visible="false"/>                
            </td>                              
            <td><asp:LinkButton ID="lbJobTitle" ValidationGroup="Input" runat="server" Text="Job Title"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlJobTitle" enabled = "False"  Width="200px" />                
                <asp:TextBox runat="server" ID="TextBox4" Visible="false"/>                
            </td>                    
          </tr>  
          <tr>   
          <td> <asp:LinkButton ID="lbEmpStatus" ValidationGroup="Input" runat="server" Text="Employee Status"/></td>
            <td>:</td>
            <td>     
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlEmpStatus" enabled = "False"  Width="200px" />                          
                <asp:TextBox runat="server" ID="TextBox5" Visible="false"/>                
            </td>                    
            <td> Gender </td>
            <td>:</td>
            <td>
                <asp:TextBox runat="server" ID="tbJenisKelamin" CssClass="TextBoxR" EnableTheming="True" width = "100" Enabled="false" />                
            </td>                              
          </tr>  
          <tr>
            <td>
                      Hospital</td>
                  <td>
                      :</td>
                  <td>                     
                    <asp:TextBox ID="tbKlinikRS" runat="server" CssClass="TextBox" 
                          MaxLength="60" ValidationGroup="Input" Width="250px" />              
                  </td>
              <td>
                  Work Period</td>
              <td>
                 :</td>
               <td>
                <asp:DropDownList ID="ddlMasaKerja" CssClass="DropDownList" ValidationGroup="Input" runat="server" 
                    Width="100px">
                      <asp:ListItem>1 - 3 Bulan</asp:ListItem>
                      <asp:ListItem>4 - 11 Bulan</asp:ListItem>
                      <asp:ListItem>1 - 3 Tahun</asp:ListItem>
                      <asp:ListItem> &gt; 3 Tahun</asp:ListItem>
                 </asp:DropDownList>
                 <asp:TextBox runat="server" ID="TextBox1" Visible="false"/>                
              </td>
          </tr>
           <tr>
              <td>
                  Accident Place</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbLokasi" runat="server" CssClass="TextBox" 
                          MaxLength="50" ValidationGroup="Input" Width="250px" />
                  </td>            
                  <td>
                      Accident Damage</td>
                  <td>
                      :</td>
                  <td>      
                     <asp:DropDownList ID="ddlJenisKecelakaan" CssClass="DropDownList" 
                          ValidationGroup="Input" runat="server" 
                       Width="133px" Height="16px">
                        <asp:ListItem>Fatal</asp:ListItem>
                        <asp:ListItem>Perawatan Medis</asp:ListItem>
                        <asp:ListItem>Kehilangan Hari Kerja</asp:ListItem>
                        <asp:ListItem>P3K</asp:ListItem>
                     </asp:DropDownList>
                     <asp:TextBox runat="server" ID="tbJenisKecelakaan" Visible="false"/>                
                  </td>
              </tr>  
              <tr>
                  <td>
                      Moment of Accident</td>
                  <td>
                      :</td>
                  <td>
                     <asp:DropDownList ID="ddlSaatKecelakaan" CssClass="DropDownList" ValidationGroup="Input" runat="server" 
                       Width="100px">
                        <asp:ListItem>Berangkat</asp:ListItem>
                        <asp:ListItem>Perawatan</asp:ListItem>
                        <asp:ListItem>Jam Lembur</asp:ListItem>
                        <asp:ListItem>Pulang Kerja</asp:ListItem>
                        <asp:ListItem>Jam Istirahat</asp:ListItem>                        
                     </asp:DropDownList>                     
                     <asp:TextBox runat="server" ID="tbSaatKecelakaan" Visible="false"/>                
                  </td>              
                  <td>
                     Injury Treatment</td>
                  <td>
                      :</td>
                  <td>                     
                     <asp:DropDownList ID="ddlPerawatan" CssClass="DropDownList" 
                          ValidationGroup="Input" runat="server" 
                       Width="133px" Height="16px">
                        <asp:ListItem>Rawat Jalan</asp:ListItem>
                        <asp:ListItem>Rawat Inap</asp:ListItem>                        
                     </asp:DropDownList>  
                     <asp:TextBox runat="server" ID="TextBox10" Visible="false"/>                
                  </td>
              </tr>                            
                  
              
              <tr>
                <td>
                   Witness Name </td>
                <td>
                   :</td>
                 <td>
                  <asp:TextBox ID="tbNamaSaksi" runat="server" CssClass="TextBox" 
                          MaxLength="60" ValidationGroup="Input" Width="250px" />
                  </td>              
                <td>
                   Witness Organization</td>
                <td>
                   :</td>
                 <td>
                  <asp:TextBox ID="tbDepartmentSaksi" runat="server" CssClass="TextBox" 
                          MaxLength="60" ValidationGroup="Input" Width="250px" />
                  </td>                 
              </tr>   
              <tr>
                <td>
                   Part of Body Injured  </td>
                <td>
                   :</td>
                 <td colspan="4">
                  <asp:TextBox ID="tbBagTubuh" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                          MaxLength="255" ValidationGroup="Input" Width="380px" />
                  </td>
              </tr>              
              <tr>
                <td>
                   Accident Description </td>
                <td>
                   :</td>
                 <td colspan="4">
                  <asp:TextBox ID="tbUrutanKejadian" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                          MaxLength="255" ValidationGroup="Input" Width="380px" />
                  </td>
              </tr>              
                                       
              <tr>
                  <td>
                      Unsafe Condition</td>
                  <td>
                      :</td>
                  <td colspan="4">
                      <asp:TextBox ID="tbKondisi" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                           ValidationGroup="Input" Width="380px" />
                  </td>
             </tr>              
             <tr>
                  <td>
                      Unsafe Acts</td>
                  <td>
                      :</td>
                  <td colspan="4">
                      <asp:TextBox ID="tbTindakan" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                          ValidationGroup="Input" Width="380px" />
                  </td>
             </tr>                           
             <tr>
                  <td>
                      Manager Advice</td>
                  <td>
                      :</td>
                  <td colspan="4">
                      <asp:TextBox ID="tbSaranManager" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                          ValidationGroup="Input" Width="380px" />
                  </td>
             </tr>                                                     
             <tr>
                  <td>
                      Safety Advice</td>
                  <td>
                      :</td>
                  <td colspan="4">
                      <asp:TextBox ID="tbSaranSafety" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                          ValidationGroup="Input" Width="380px" />
                  </td>
             </tr>                                     
             <tr>
                  <td>
                      Remark</td>
                  <td>
                      :</td>
                  <td colspan="4">
                      <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                          ValidationGroup="Input" Width="380px" />
                  </td>
             </tr>
        </table> 
        <br />  
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	       
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
        <div>
            <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
                AutoWidth="True" Height="100%" Width="100%" />
        </div>
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
