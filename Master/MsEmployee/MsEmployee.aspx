<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsEmployee.aspx.vb" Inherits="Master_MsEmployee_MsEmployee" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
    function OpenPopup() {         
            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }   
    function OpenPopup2() {        
        window.open("../../SearchMultiDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 
    function OpenPopupSearch() {         
            window.open("../../UserControl/AdvanceSearch.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }       
    function openprintdlg7ds() {         
            window.open("../../Rpt/PrintForm7.Aspx","List","scrollbars=yes,resizable=yes,width=500,height=400");        
            return false;
        }    
    function addCommas(nStr)
    {
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	}
	function setdigit(nStr, digit) {
	    try {
	        var TNstr = parseFloat(nStr);
	        TNstr = TNstr.toFixed(digit);
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
	    } catch (err) {
	        alert(err.description);
	    }
	}
    function setformatDt() {
        try {
            var _tempsalaryExp = parseFloat(document.getElementById("tbsalaryExp").value.replace(/\$|\,/g, ""));

            document.getElementById("tbsalaryExp").value = setdigit(_tempsalaryExp, '<%=ViewState("DigitCurr")%>');
            
        } catch (err) {
            alert(err.description);
        }
    }
    
</script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 104px;
        }
        .style2
        {
            width: 8px;
        }
        .style3
        {
            width: 347px;
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
        }
        </style>
  
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Employee File</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="EmpNumb">NIK</asp:ListItem>
                      <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                      <asp:ListItem Value="ApplicantNo">Candidate No</asp:ListItem>
                      <asp:ListItem Value="Gender">Gender</asp:ListItem>                      
                      <asp:ListItem Value="BirthPlace">Birth Place</asp:ListItem>
                      <asp:ListItem Value="BirthDate">Birth Date</asp:ListItem>
                      <asp:ListItem Value="HireDate">Start Date</asp:ListItem>
                      <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                      <asp:ListItem Value="ReligionName">Religion</asp:ListItem>
                      <asp:ListItem Value="Tribe">Tribe</asp:ListItem>
                      <asp:ListItem Value="ResAddr1">Res Addr 1</asp:ListItem>
                      <asp:ListItem Value="ResAddr2">Res Addr 2</asp:ListItem>
                      <asp:ListItem Value="ResCity">Res City</asp:ListItem>
                      <asp:ListItem Value="ResPhone">Res Phone</asp:ListItem>
                      <asp:ListItem Value="ResAddrStatus">Res Addr Status</asp:ListItem>
                      <asp:ListItem Value="OriAddr1">Ori Addr 1</asp:ListItem>
                      <asp:ListItem Value="OriAddr2">Ori Addr 2</asp:ListItem>
                      <asp:ListItem Value="OriCity">Ori City</asp:ListItem>
                      <asp:ListItem Value="OriPhone">Ori Phone</asp:ListItem>
                      <asp:ListItem Value="TypeCard">Type Card</asp:ListItem>
                      <asp:ListItem Value="IdCard">ID Card</asp:ListItem>
                      <asp:ListItem Value="BloodType">Blood Type</asp:ListItem>
                      <asp:ListItem Value="AbsenceCard">Absence Card</asp:ListItem>
                      <asp:ListItem Value="Weight">Weight</asp:ListItem>
                      <asp:ListItem Value="Height">Height</asp:ListItem>
                      <asp:ListItem Value="Handphone">Phone</asp:ListItem>
                      <asp:ListItem Value="Email">Email</asp:ListItem>
                      <asp:ListItem Value="LastCertificateNo">Last Certificate No</asp:ListItem>
                      <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                      <asp:ListItem Value="MaritalTax">Marital Tax</asp:ListItem>
                      <asp:ListItem Value="MaritalSt">Marital St</asp:ListItem>
                      <asp:ListItem Value="MaritalDate">Marital Date</asp:ListItem>
                      <asp:ListItem Value="MaritalDocNo">Marital Doc No</asp:ListItem>
                      <asp:ListItem Value="FgJamsosTek">Join Jamsostek</asp:ListItem>
                      <asp:ListItem Value="JamsostekNo">Jamsostek No</asp:ListItem>
                      <asp:ListItem Value="Jamsostekdate">Jamsostek Date</asp:ListItem>
                      <asp:ListItem Value="AKDHKNo">AKDHK No</asp:ListItem>
                      <asp:ListItem Value="SalaryType">Salary Type</asp:ListItem>
                      <asp:ListItem Value="PinBB">Pin BB</asp:ListItem>
                      <asp:ListItem Value="FgTKA">TKA</asp:ListItem>
                      <asp:ListItem Value="TKAStatus">Status</asp:ListItem>
                      <asp:ListItem Value="FgActive">Active</asp:ListItem>
                      <asp:ListItem Value="JobLvlName">Job Level</asp:ListItem>
                      <asp:ListItem Value="JobTtlName">Job Title</asp:ListItem>
                      <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                      <%--<asp:ListItem Value="Section_Name">Section</asp:ListItem>
                      <asp:ListItem Value="Sub_Section_Name">Sub Section</asp:ListItem>--%>
                      <asp:ListItem Value="EmpStatusName">Emp Status</asp:ListItem>
                      <asp:ListItem Value="WorkPlaceName">Work Place</asp:ListItem>
                      <asp:ListItem Value="SKNo">SK No</asp:ListItem>
                      <asp:ListItem Value="EndDateContract">End Contract</asp:ListItem>
                    </asp:DropDownList>
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnsearch" Text="Search" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnexpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnprint" Text="Print"/>
               </td>
            <td>
                &nbsp;</td>
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
                 <asp:ListItem Selected="True" Value="EmpNumb">NIK</asp:ListItem>
                      <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                      <asp:ListItem Value="ApplicantNo">Candidate No</asp:ListItem>
                      <asp:ListItem Value="Gender">Gender</asp:ListItem>                      
                      <asp:ListItem Value="BirthPlace">Birth Place</asp:ListItem>
                      <asp:ListItem Value="BirthDate">Birth Date</asp:ListItem>
                      <asp:ListItem Value="HireDate">Start Date</asp:ListItem>
                      <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                      <asp:ListItem Value="ReligionName">Religion</asp:ListItem>
                      <asp:ListItem Value="Tribe">Tribe</asp:ListItem>
                      <asp:ListItem Value="ResAddr1">Res Addr 1</asp:ListItem>
                      <asp:ListItem Value="ResAddr2">Res Addr 2</asp:ListItem>
                      <asp:ListItem Value="ResCity">Res City</asp:ListItem>
                      <asp:ListItem Value="ResPhone">Res Phone</asp:ListItem>
                      <asp:ListItem Value="ResAddrStatus">Res Addr Status</asp:ListItem>
                      <asp:ListItem Value="OriAddr1">Ori Addr 1</asp:ListItem>
                      <asp:ListItem Value="OriAddr2">Ori Addr 2</asp:ListItem>
                      <asp:ListItem Value="OriCity">Ori City</asp:ListItem>
                      <asp:ListItem Value="OriPhone">Ori Phone</asp:ListItem>
                      <asp:ListItem Value="TypeCard">Type Card</asp:ListItem>
                      <asp:ListItem Value="IdCard">ID Card</asp:ListItem>
                      <asp:ListItem Value="BloodType">Blood Type</asp:ListItem>
                      <asp:ListItem Value="AbsenceCard">Absence Card</asp:ListItem>
                      <asp:ListItem Value="Weight">Weight</asp:ListItem>
                      <asp:ListItem Value="Height">Height</asp:ListItem>
                      <asp:ListItem Value="Handphone">Phone</asp:ListItem>
                      <asp:ListItem Value="Email">Email</asp:ListItem>
                      <asp:ListItem Value="LastCertificateNo">Last Certificate No</asp:ListItem>
                      <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                      <asp:ListItem Value="MaritalTax">Marital Tax</asp:ListItem>
                      <asp:ListItem Value="MaritalSt">Marital St</asp:ListItem>
                      <asp:ListItem Value="MaritalDate">Marital Date</asp:ListItem>
                      <asp:ListItem Value="MaritalDocNo">Marital Doc No</asp:ListItem>
                      <asp:ListItem Value="FgJamsosTek">Join Jamsostek</asp:ListItem>
                      <asp:ListItem Value="JamsostekNo">Jamsostek No</asp:ListItem>
                      <asp:ListItem Value="Jamsostekdate">Jamsostek Date</asp:ListItem>
                      <asp:ListItem Value="AKDHKNo">AKDHK No</asp:ListItem>
                      <asp:ListItem Value="SalaryType">Salary Type</asp:ListItem>
                      <asp:ListItem Value="PinBB">Pin BB</asp:ListItem>
                      <asp:ListItem Value="FgTKA">TKA</asp:ListItem>
                      <asp:ListItem Value="TKAStatus">Status</asp:ListItem>
                      <asp:ListItem Value="FgActive">Active</asp:ListItem>
                      <asp:ListItem Value="JobLvlName">Job Level</asp:ListItem>
                      <asp:ListItem Value="JobTtlName">Job Title</asp:ListItem>
                      <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                      <%--<asp:ListItem Value="Section_Name">Section</asp:ListItem>
                      <asp:ListItem Value="Sub_Section_Name">Sub Section</asp:ListItem>--%>
                      <asp:ListItem Value="EmpStatusName">Emp Status</asp:ListItem>
                      <asp:ListItem Value="WorkPlaceName">Work Place</asp:ListItem>
                      <asp:ListItem Value="SKNo">SK No</asp:ListItem>
                      <asp:ListItem Value="EndDateContract">End Contract</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
             <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" />									
            <br />
          <div style="border:0px  solid; width:100%; height:500px; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField Visible="false">
                              <ItemTemplate>
                                  <asp:Label ID="cbSelectHd" runat="server" 
                                      text='' />
                              </ItemTemplate>
                        </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />                              
                              <asp:ListItem>Delete</asp:ListItem>
                              <asp:ListItem>Copy New</asp:ListItem>
                              <asp:ListItem Text="Photo" />
                              <asp:ListItem Text="Print" />                                
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="btngo" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="EmpNumb" HeaderText="NIK" SortExpression="EmpNumb"/>
                  <asp:BoundField DataField="EmpName" HeaderText="Employee Name" SortExpression="EmpName"/>                  
                  <asp:BoundField DataField="ApplicantNo" HeaderText="Candidate No" SortExpression="ApplicantNo"/>                  
                  <asp:BoundField DataField="Gender" HeaderText="Gender" 
                      SortExpression="Gender" />
                    <asp:BoundField DataField="BirthPlace" HeaderText="Birth Place" 
                      SortExpression="BirthPlace" />
                    <asp:BoundField DataField="BirthDate" HeaderText="Birth Date " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="BirthDate" />
                    <asp:BoundField DataField="HireDate" HeaderText="Start Date " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="HireDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="EndDate" />
                    <asp:BoundField DataField="ReligionName" HeaderText="Religion " 
                                          SortExpression="ReligionName" />
                    <asp:BoundField DataField="Tribe" HeaderText="Tribe " 
                                          SortExpression="Tribe" />
                    <asp:BoundField DataField="ResAddr1" HeaderText="Res Addr 1" 
                                          SortExpression="ResAddr1" />
                    <asp:BoundField DataField="ResAddr2" HeaderText="ResAddr 2" 
                                          SortExpression="ResAddr2" />
                    <asp:BoundField DataField="ResCity" HeaderText="Res City " 
                                          SortExpression="ResCity" />
                    <asp:BoundField DataField="ResPostCode" HeaderText="Res Zip Code" 
                                          SortExpression="ResPostCode" />
                    <asp:BoundField DataField="ResPhone" HeaderText="Res Phone " 
                                          SortExpression="ResPhone" />
                    <asp:BoundField DataField="ResAddrStatus" HeaderText="Res Addr Status " 
                                          SortExpression="ResAddrStatus" />
                    <asp:BoundField DataField="OriAddr1" HeaderText="Ori Addr 1" 
                                          SortExpression="OriAddr1" />
                    <asp:BoundField DataField="OriAddr2" HeaderText="OriAddr 2" 
                                          SortExpression="OriAddr2" />
                    <asp:BoundField DataField="OriCity" HeaderText="Ori City " 
                                          SortExpression="OriCity" />
                    <asp:BoundField DataField="OriPostCode" HeaderText="Ori Zip Code " 
                                          SortExpression="OriPostCode" />
                    <asp:BoundField DataField="OriPhone" HeaderText="Ori Phone " 
                                          SortExpression="OriPhone" />
                    <asp:BoundField DataField="TypeCard" HeaderText="Type Card " 
                                          SortExpression="TypeCard" />
                    <asp:BoundField DataField="IdCard" HeaderText="Id Card " 
                                          SortExpression="IdCard" />
                    <asp:BoundField DataField="BloodType" HeaderText="Blood Type " 
                                          SortExpression="BloodType" />
                    <asp:BoundField DataField="Weight" HeaderText="Weight" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" 
                                          SortExpression="Weight" />
                    <asp:BoundField DataField="Height" HeaderText="Height" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" 
                                          SortExpression="Height" />
                    <asp:BoundField DataField="Handphone" HeaderText="Handphone " 
                                          SortExpression="Handphone" />
                    <asp:BoundField DataField="Email" HeaderText="Email" 
                                          SortExpression="Email" />
                    <asp:BoundField DataField="LastCertificateNo" HeaderText="Last Certificate No " 
                                          SortExpression="LastCertificateNo" />
                    <asp:BoundField DataField="NPWP" HeaderText="NPWP " 
                                          SortExpression="NPWP" />
                    <asp:BoundField DataField="MaritalTax" HeaderText="Marital Tax " 
                                          SortExpression="MaritalTax" />
                    <asp:BoundField DataField="MaritalSt" HeaderText="Marital St " 
                                          SortExpression="MaritalSt" />
                    <asp:BoundField DataField="MaritalDate" HeaderText="Marital Date " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="MaritalDate" />
                    <asp:BoundField DataField="MaritalDocNo" HeaderText="Marital Doc No " 
                                          SortExpression="MaritalDocNo" />
                    <asp:BoundField DataField="FgJamsostek" HeaderText="Join Jamsostek" 
                                          SortExpression="FgJamsostek" />
                    <asp:BoundField DataField="JamsostekNo" HeaderText="Jamsostek No" 
                                          SortExpression="JamsostekNo" />
                    <asp:BoundField DataField="Jamsostekdate" HeaderText="Jamsostek Date " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="Jamsostekdate" />
                    <asp:BoundField DataField="AKDHKNo" HeaderText="AKDHK No " 
                                          SortExpression="AKDHKNo" />
                    <asp:BoundField DataField="SalaryType" HeaderText="Salary Type " 
                                          SortExpression="SalaryType" />

                    <asp:BoundField DataField="PinBB" HeaderText="Pin BB " 
                                          SortExpression="PinBB" />
                    <asp:BoundField DataField="FgTKA" HeaderText="TKA " 
                                          SortExpression="FgTKA" />
                    <asp:BoundField DataField="TKAStatus" HeaderText="Status " 
                                          SortExpression="TKAStatus" />
                    <asp:BoundField DataField="FgActive" HeaderText="Active " 
                                          SortExpression="FgActive" />
                    <asp:BoundField DataField="Job_Level" HeaderText="Job Level " 
                                          SortExpression="Job_Level" />
                    <asp:BoundField DataField="Job_Title" HeaderText="Job Title " 
                                          SortExpression="Job_Title" />
                    <asp:BoundField DataField="Dept_Name" HeaderText="Organization" 
                                          SortExpression="Dept_Name" />
                    <%--<asp:BoundField DataField="Section_Name" HeaderText="Section" 
                                          SortExpression="Section_Name" />                                            
                    <asp:BoundField DataField="Sub_Section_Name" HeaderText="Sub Section" 
                                          SortExpression="Sub_Section_Name" />         --%>             
                    <asp:BoundField DataField="Emp_Status_Name" HeaderText="Emp Status " 
                                          SortExpression="Emp_Status_Name" />
                    <asp:BoundField DataField="Work_Place" HeaderText="Work Place " 
                                          SortExpression="Work_Place" />
                    <asp:BoundField DataField="SKNo" HeaderText="SK No " 
                                          SortExpression="SKNo" />
                    <asp:BoundField DataField="EndDateContract" HeaderText="End Contract " DataFormatString="{0:dd MMM yyyy}" 
                                          SortExpression="EndDateContract" />

                  </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadd2" Text="Add" />									            
            </asp:Panel>
    </asp:Panel>    
        
       <asp:Panel ID="pnlView" runat="server" Visible="false"> 
       <br />
       <asp:Menu ID="Menu2" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="General" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Job Info" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Contact" Value="2"></asp:MenuItem>
                <asp:MenuItem Text="Others" Value="3"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br /> 
         <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="Tab0" runat="server">
                       <table>
                            <tr>
                                <td >
                                    NIK </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCode" runat="server" CssClass="TextBox" MaxLength="15" 
                                        Width="149px" /> 
                                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Employee Name</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" MaxLength="100" 
                                        Width="200px" />
                                    &nbsp;<asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                            </tr>
                            <td >
                                    Foto </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:image id="imgUpload" bordercolor="Black" 
                                                runat="server" width="150px"  />

                                      



                                   <%-- <asp:FileUpload runat="server" ID="FupImage"  />
                                        <asp:RegularExpressionValidator ID="FileUpLoadValidator" runat="server" ErrorMessage="Image Files Only (.jpg, .bmp, .png, .gif)"
                                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.gif|.GIF|.jpeg|.JPEG|.bmp|.BMP|.png|.PNG)$" 
                                           ControlToValidate="FupImage"> 
                                        </asp:RegularExpressionValidator>
                                        <asp:Label Visible="False" runat="server" ID="lbGroup" Font-Bold="true"></asp:Label>  --%>
                                </td>
                            <tr>
                                <td >
                                    Candidate No</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCandidate" runat="server" CssClass="TextBoxR" MaxLength="20"
                                        Width="200px" Enabled="False" />
                                    <asp:Button ID="btnCandidate" runat="server" class="btngo" Text="..." />
                                    &nbsp; &nbsp;
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Blood Type
                                </td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:DropDownList ID="ddlBloodType" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="91px">
                                        <asp:ListItem Value="">Choose One</asp:ListItem>
                                        <asp:ListItem>A</asp:ListItem>
                                        <asp:ListItem>B</asp:ListItem>
                                        <asp:ListItem>O</asp:ListItem>
                                        <asp:ListItem>AB</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Gender </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="69px">
                                        <asp:ListItem>Female</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Weight</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:TextBox ID="tbWeight" runat="server" CssClass="TextBox" Width="103px" MaxLength="16" />
                                    kg</td>
                            </tr>
                            <tr>
                                <td >
                                    Bitrh Place</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbBirthPlace" runat="server" CssClass="TextBox" MaxLength="60" 
                                        Width="200px" />
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Height</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:TextBox ID="tbHeight" runat="server" CssClass="TextBox" MaxLength="16" 
                                        Width="58px" />
                                    cm</td>
                            </tr>
                            <tr>
                                <td >
                                    Birth Date</td>
                                <td >
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbBirthDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                  
                                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                  
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Marital Status</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:DropDownList ID="ddlMaritalSt" runat="server" CssClass="DropDownList" 
                                        Width="84px" Height="16px" >
                                        <asp:ListItem Value="">Choose One</asp:ListItem>
                                        <asp:ListItem>Single</asp:ListItem>
                                        <asp:ListItem>Married</asp:ListItem>
                                        <asp:ListItem>Widowed</asp:ListItem>
                                        <asp:ListItem>Divorce</asp:ListItem>
                                        <asp:ListItem>Separated</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label19" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Religion </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlReligion" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Marital Tax</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:DropDownList ID="ddlMaritalTax" runat="server" CssClass="DropDownList" 
                                        Width="84px" Height="16px">
                                        <asp:ListItem Value ="">Choose One</asp:ListItem>
                                        <asp:ListItem>TK 0</asp:ListItem>
                                        <asp:ListItem>TK 1</asp:ListItem>
                                        <asp:ListItem>TK 2</asp:ListItem>
                                        <asp:ListItem>TK 3</asp:ListItem>
                                        <asp:ListItem>K 0</asp:ListItem>
                                        <asp:ListItem>K 1</asp:ListItem>
                                        <asp:ListItem>K 2</asp:ListItem>
                                        <asp:ListItem>K 3</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Tribe</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbTribe" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="219px" />
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Marital Doc. No</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:TextBox ID="tbMaritalDocNo" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Absence Card No.</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCard" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="200px" />
                                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Marital Date</td>
                                <td >
                                    :</td>
                                <td >
                                    <BDP:BasicDatePicker ID="tbDateMarital" runat="server" AutoPostBack="True" 
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                        DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Type Card</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlTypeCard" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="147px">
                                        <asp:ListItem>KTP</asp:ListItem>
                                        <asp:ListItem>SIM A</asp:ListItem>
                                        <asp:ListItem>SIM B</asp:ListItem>
                                        <asp:ListItem>SIM C</asp:ListItem>
                                        <asp:ListItem>Passport</asp:ListItem>
                                        <asp:ListItem>Kartu Pelajar</asp:ListItem>
                                        <asp:ListItem>Kartu Mahasiswa</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    Active</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:DropDownList ID="ddlActive" runat="server" CssClass="DropDownList" 
                                        Width="44px" Enabled="false">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    ID Card</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbIDCard" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="200px" />
                                    <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    SK No.</td>
                                <td >
                                    :</td>
                                <td >
                                    <asp:TextBox ID="tbSKNo" runat="server" CssClass="TextBox" MaxLength="18" 
                                        Width="200px" />
                                </td>
                            </tr>                    
                            
                        </table>

         </asp:View> 
         
         <asp:View ID="Tab1" runat="server">
                       <table>
                       <tr>
                                <td >
                                    Start Date</td>
                                <td >
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                  
                                    <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                  
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                       <tr>
                                <td >
                                    End Date</td>
                                <td >
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" Enabled="false"
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                        DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                  
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr> 
                         
                       <tr>
                                <td >
                                    Job Title </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                       <tr>
                                <td >
                                    Job Level </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlJobLevel" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label13" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr> 
                            <tr>
                                <td >
                                    Organization </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label14" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                       <%--<tr>
                                <td >
                                    Section</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlsection" runat="server" CssClass="DropDownList" 
                                        Width="200px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>--%>
                       <%--<tr>
                                <td >
                                    Sub Section</td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlSubSection" runat="server" CssClass="DropDownList" 
                                        Width="200px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label31" runat="server" ForeColor="Red" Text="*"></asp:Label>
                  
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>--%>  
                            
                       <tr>
                                <td >
                                    Employee Status </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlEmpStatus" runat="server" CssClass="DropDownList" 
                                        Width="200px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>     
                       <tr>
                                <td >
                                    End Date Contract</td>
                                <td >
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbEndContract" runat="server" AutoPostBack="True" 
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                        DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                       <tr>
                                <td >
                                    Work Place </td>
                                <td >
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlWorkPlace" runat="server" CssClass="DropDownList" 
                                        Width="200px" >
                                    </asp:DropDownList>
                                    <asp:Label ID="Label16" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                                <td >
                                    &nbsp;</td>
                            </tr>
                                                       
                           <tr>
                               <td>
                                   Salary Type</td>
                               <td>
                                   :</td>
                               <td>
                                   <asp:DropDownList ID="ddlSalaryType" runat="server" CssClass="DropDownList" 
                                       Height="16px" Width="147px">
                                       <asp:ListItem>Transfer</asp:ListItem>
                                       <asp:ListItem>Tunai</asp:ListItem>
                                   </asp:DropDownList>
                               </td>
                               <td >
                                   &nbsp;</td>
                               <td >
                                   &nbsp;</td>
                               <td >
                                   &nbsp;</td>
                           </tr>
                           
                           <tr>
                               <td>
                                   Method Salary</td>
                               <td>
                                   :</td>
                               <td>
                                   <asp:DropDownList ID="ddlMethod" runat="server" CssClass="DropDownList" Height="16px" Width="147px">
                                   </asp:DropDownList>
                               </td>
                               <td >
                                   &nbsp;</td>
                               <td >
                                   &nbsp;</td>
                               <td >
                                   &nbsp;</td>
                           </tr>
                                                       
                           <tr>
                               <td>
                                   Sales</td>
                               <td>
                                   :</td>
                               <td>
                                   <asp:DropDownList ID="ddlSales" runat="server" CssClass="DropDownList" 
                                       Enabled="false" Height="16px" Width="90px">
                                       <asp:ListItem Value="">Choose One</asp:ListItem>
                                       <asp:ListItem>Y</asp:ListItem>
                                       <asp:ListItem>N</asp:ListItem>
                                   </asp:DropDownList>
                               </td>
                               <td>
                                   &nbsp;</td>
                               <td>
                                   &nbsp;</td>
                               <td>
                                   &nbsp;</td>
                           </tr>
                                                       
                        </table>

         </asp:View>
         
         <asp:View ID="Tab2" runat="server">
                       <table>
                            

                            <tr>
                                <td class="style1">
                                    Telephone</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbPhoneContact" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="198px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td class="style3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Email</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" MaxLength="100" 
                                        Width="198px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td class="style3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Pin BB</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbPinBB" runat="server" CssClass="TextBox" MaxLength="20" 
                                        Width="198px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td class="style3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Res Addr 1 </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbResAddr1" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="199px"/>
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style1">
                                    Ori Addr 1 </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbOriAddr1" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="199px"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Res Addr 2 </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbResAddr2" runat="server" CssClass="TextBox" Width="198px" 
                                        MaxLength="255"/>
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style1">
                                    Ori Addr 2 </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbOriAddr2" runat="server" CssClass="TextBox" Width="198px" 
                                        MaxLength="255"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Res Zip Code </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbResZipCode" runat="server" CssClass="TextBox" Width="80px" 
                                        MaxLength="5"/>
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style1">
                                    Ori Zip Code </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbOriZipCode" runat="server" CssClass="TextBox" Width="80px" 
                                        MaxLength="5"/>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="style1">
                                   Res City </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlResCity" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td class="style4">
                                    &nbsp;&nbsp;</td>
                                <td class="style1">
                                   Ori City </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlOriCity" runat="server" CssClass="DropDownList" 
                                        Width="200px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="style1">
                                    Res Phone</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbResPhone" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="198px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style1">
                                    Ori Phone</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbOriPhone" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="198px" />
                                </td>
                                
                            </tr>  
                            
                            <tr>
                                <td class="style1">
                                    Res Addr Status</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                 <asp:DropDownList ID="ddlResAddrStatus" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="102px">
                                        <asp:ListItem Value="">Choose One</asp:ListItem>
                                        <asp:ListItem>Own</asp:ListItem>
                                        <asp:ListItem>Contract</asp:ListItem>
                                        <asp:ListItem>Other</asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            </table>
                            </asp:View>

        <asp:View ID="Tab3" runat="server">
                       <table>
                            <tr>
                                <td class="style1">
                                    NPWP </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbNPWP" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="149px" />
                                    &nbsp; &nbsp;
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Join Jamsostek </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlFgJamsosTek" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="69px">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Jamsostek No</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbjamsostek" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="200px" />
                                    &nbsp; &nbsp;
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;
                                </td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Jamsostek Date</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbJamsostekDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                  
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>                           
                            <tr>
                                <td class="style1">
                                    AKDHK No</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbAKDHK" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="200px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            
                            <tr>
                                <td class="style1">
                                    TKA </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlTKA" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="69px">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Status </td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="117px">
                                        <asp:ListItem Value="">Choose One</asp:ListItem>
                                        <asp:ListItem>Permanent</asp:ListItem>
                                        <asp:ListItem>Non Permanent</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Last Sertificate No</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbLastNo" runat="server" CssClass="TextBox" MaxLength="30" 
                                        Width="200px" />
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                            </tr>
                            
                        </table>

         </asp:View>                 
         </asp:MultiView>
         
           
           <table>
               <tr>
                   <td class="style1">
                       &nbsp;</td>
                   <td class="style2">
                       &nbsp;</td>
                   <td>
                       <asp:Button ID="btnSaveHd" runat="server" class="bitbtndt btnsave" 
                           Text="Save" />
                       <asp:Button ID="btnCancelHd" runat="server" class="bitbtndt btncancel" 
                           Text="Cancel" />
                       <asp:Button ID="btnReset" runat="server" class="bitbtndt btnback" 
                           Text="Reset" />
                   </td>
                   <td class="style4">
                       &nbsp;</td>
                   <td class="style6">
                       &nbsp;</td>
                   <td class="style5">
                       &nbsp;</td>
                   <td class="style3">
                       &nbsp;</td>
               </tr>
           </table>
         
        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBack" Text="Back" />									            
        
        <br />
        <br />
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Language" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Family" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Skill" Value="2"></asp:MenuItem>
                <asp:MenuItem Text="Education" Value="3"></asp:MenuItem>
                <asp:MenuItem Text="Experience" Value="4"></asp:MenuItem>
                <asp:MenuItem Text="Training" Value="5"></asp:MenuItem>
                <asp:MenuItem Text="Achievement" Value="6"></asp:MenuItem>
                <asp:MenuItem Text="Bank" Value="7"></asp:MenuItem>
                <asp:MenuItem Text="Emergency" Value="8"></asp:MenuItem>
                <asp:MenuItem Text="Society" Value="9"></asp:MenuItem>
                <asp:MenuItem Text="Hobby" Value="10"></asp:MenuItem>
                <asp:MenuItem Text="Memo" Value="11"></asp:MenuItem>
            </Items>
        </asp:Menu>
        
        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
            <asp:View ID="TabLanguage" runat="server">
               <br> 
                <asp:Panel ID="pnlDt" runat="server">
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" Text="Add" CommandName = 'Insert'/>									                                
                   
                    <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                        <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" AllowSorting="True" 
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditDt" Text="Edit" CommandNAme="Edit"  />									                                
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteDt" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="ItemNo"/>
                                <asp:BoundField DataField="LanguageName" HeaderText="Language" 
                                    SortExpression="LanguageName" />
                                <asp:BoundField DataField="GradeRead" HeaderText="Reading" SortExpression="GradeRead"/>
                                <asp:BoundField DataField="GradeWrite" HeaderText="Writing" SortExpression="GradeWrite" />
                                <asp:BoundField DataField="GradeSpeak" HeaderText="Speaking" SortExpression="GradeSpeak" />
                                <asp:BoundField DataField="Exam" HeaderText="Language Exam" SortExpression="Exam" />
                                <asp:BoundField DataField="GPD" HeaderText="GPD" SortExpression="GPD" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNo" runat="server" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Language</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbLanguage" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Reading</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlRead" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="75px">
                                        <asp:ListItem>Poor</asp:ListItem>
                                        <asp:ListItem>Good</asp:ListItem>
                                        <asp:ListItem>Excelent</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Writing</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlWrite" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="75px">
                                        <asp:ListItem>Poor</asp:ListItem>
                                        <asp:ListItem>Good</asp:ListItem>
                                        <asp:ListItem>Excelent</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Speaking</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlSpeak" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="75px">
                                        <asp:ListItem>Poor</asp:ListItem>
                                        <asp:ListItem>Good</asp:ListItem>
                                        <asp:ListItem>Excelent</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Language Exam</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbexam" runat="server" CssClass="TextBox" Width="269px" MaxLength="50"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    GPD</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbGPD" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="198px" />
                                </td>
                            </tr>
                             
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />
                 </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabFamily" runat="server">
               <br> 
                <asp:Panel ID="Pnlfamily" runat="server">
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddf" Text="Add" CommandName = 'Insert'/>									                                
                   
                    <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                        <asp:GridView ID="Gridf" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditf" Text="Edit" CommandNAme="Edit"  />									                                
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeletef" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="ItemNo"/>
                                <asp:BoundField DataField="FamilyType" HeaderText="Family Type" SortExpression="FamilyType" />
                                <asp:BoundField DataField="FamilyName" HeaderText="Family Name" SortExpression="FamilyName"/>
                                <asp:BoundField DataField="Occupation" HeaderText="Occupation" SortExpression="Occupation"/>
                                <asp:BoundField DataField="Address1" HeaderText="Address 1" SortExpression="Address1"/>
                                <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/>
                                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone"/>
                                <asp:BoundField DataField="BirthPlace" HeaderText="Birth Place" SortExpression="BirthPlace"/>
                                <asp:BoundField DataField="BirthDate" HeaderText="Birth Date" DataFormatString="{0:dd MMM yyyy}" SortExpression="BirthDate"/>
                                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender"/>
                                <asp:BoundField DataField="EduLevel" HeaderText="Last Education" SortExpression="EduLevel"/>
                                <asp:BoundField DataField="Fgmedical" HeaderText="Fg Medical" SortExpression="Fgmedical"/>
                                
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
                <asp:Panel ID="PnlEditFamily" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoF" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Family Type</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlfamilyType" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="110px">
                                        <asp:ListItem>Spouse</asp:ListItem>
                                        <asp:ListItem>Children</asp:ListItem>
                                        <asp:ListItem>Parent</asp:ListItem>
                                        <asp:ListItem>Parent in Law</asp:ListItem>
                                        <asp:ListItem>Sibling</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Family Name</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbFamilyName" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Occupation</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbOccupation" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 1</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbAddress1" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 2</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbAddress2" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbPhone" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Place of Birth</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbBirthPlaceF" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Date of Birth</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbBirthDateFamily" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                  
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td class="style3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Gender</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGenderFamily" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="80px">
                                        <asp:ListItem>Female</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Last Education</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlEduLevel" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="150px">
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Claim Medical</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlFgMedical" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="44px">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsavef" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelf" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />
                 </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabSkill" runat="server">
               <br />
               <asp:Panel ID="PnlSkill" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdds" Text="Add" />									                                                                                                                                                                                                                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="GridS" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdits" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                    
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeletes" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                                                                                                                                                    
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                <asp:BoundField DataField="SkillType" HeaderText="Type" SortExpression="SkillType" />
                                <asp:BoundField DataField="SkillName" HeaderText="Skill" SortExpression="SkillName"/>
                                <asp:BoundField DataField="SkillGrade" HeaderText="Grade" SortExpression="SkillGrade"/>
                                <asp:BoundField DataField="InternalAction" HeaderText="Internal Action" SortExpression="InternalAction"/>
                                
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                
                                <br />
                            </asp:Panel> 
                            <asp:Panel ID="PnlEditSkill" runat="server" Visible="false">
                                    <table>
                             <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoSkill" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Type</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlSkillType" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="94px">
                                        <asp:ListItem>Skill</asp:ListItem>
                                        <asp:ListItem>Knowledge</asp:ListItem>
                                        <asp:ListItem>Ability</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Skill</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbSkilName" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Grade</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlSkillGrade" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="44px">
                                        <asp:ListItem>A</asp:ListItem>
                                        <asp:ListItem>B</asp:ListItem>
                                        <asp:ListItem>C</asp:ListItem>
                                        <asp:ListItem>D</asp:ListItem>
                                        <asp:ListItem>E</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Internal Action</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbInternalAction" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                                        
                                        <tr>
                                            <td colspan="3" style="text-align: center">
                                               <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaves" Text="Save" />									                                                                                                                                                        
                                               <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancels" Text="Cancel" />									                                                                                                                                                                                                                                                                            
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                     </asp:Panel>     
            </asp:View>
            
            <asp:View ID="TabEdu" runat="server">
               <br> 
                <asp:Panel ID="PnlEdu" runat="server">
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddEdu" Text="Add" CommandName = 'Insert'/>									                                
                   
                    <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                        <asp:GridView ID="GridEdu" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditEdu" Text="Edit" CommandNAme="Edit"  />									                                
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteEdu" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                        </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="EduLevel" HeaderText="Education Level" 
                                    SortExpression="EduLevel" />
                                <asp:BoundField DataField="SchoolName" HeaderText="School Name" SortExpression="SchoolName"/>
                                <asp:BoundField DataField="Address1" HeaderText="Address 1" SortExpression="Address1"/>
                                <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/>
                                <asp:BoundField DataField="EduCity" HeaderText="City" SortExpression="EduCity"/>
                                <asp:BoundField DataField="EduMajor" HeaderText="Major" SortExpression="EduMajor"/>
                                <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No" SortExpression="CertificateNo"/>
                                <asp:BoundField DataField="GPA" HeaderText="GPA" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="GPA"/>
                                <asp:BoundField DataField="DurasiYear" HeaderText="Period" SortExpression="DurasiYear"/>
                                <asp:BoundField DataField="FgGraduated" HeaderText="Fg Graduated" SortExpression="FgGraduated"/>
                                <asp:BoundField DataField="Graduated" HeaderText="Graduated" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Graduated"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlEditEdu" runat="server" Visible="false">
                        <table>                                                       
                            <tr>
                                <td>
                                    Education Level</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlEduLevelEdu" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="215px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label17" runat="server" ForeColor="Red" Text="*" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    School Name</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbSchollName" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="269px" />
                                    <asp:Label ID="Label18" runat="server" ForeColor="Red" Text="*" />    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 1</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbAddr1Edu" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 2</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbAddr2Edu" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlCityEdu" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="215px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Major</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbMajor" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Certificate No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCertificate" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    GPA</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbGPA" runat="server" CssClass="TextBox" MaxLength="16"
                                        Width="80px" AutoPostBack="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Period</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbPeriod" runat="server" CssClass="TextBox" MaxLength="40"
                                        Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fg Graduated</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGraduated" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="44px" AutoPostBack="True">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Graduated</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbGraduated" runat="server" CssClass="TextBox" MaxLength="10"
                                        Width="80px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveEdu" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelEdu" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />
                 </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabExperience" runat="server">
                <br />
                <asp:Panel ID="PnlExp" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddExp" Text="Add" />									                                                                                                                                                                                                                                
                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="GridExp" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditexp" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                                                                        
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteexp" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');" />									                                                                                                                                                                                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                               
                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="ItemNo"/>
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName"/>
                                            <asp:BoundField DataField="Companybusiness" HeaderText="Type of Business" SortExpression="Companybusiness"/>
                                            <asp:BoundField DataField="Address1" HeaderText="Address" SortExpression="Address1"/>
                                            <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/>
                                            <asp:BoundField DataField="CompanyCity" HeaderText="City" SortExpression="CompanyCity"/>
                                            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone"/>
                                            <asp:BoundField DataField="Jobtitle" HeaderText="Job Title" SortExpression="Jobtitle"/>
                                            <asp:BoundField DataField="Department" HeaderText="Dept / Division" SortExpression="Department"/>
                                            <asp:BoundField DataField="LastSalary" HeaderText="Salary" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="LastSalary"/>
                                            <asp:BoundField DataField="JobResponsibility" HeaderText="Job Description" SortExpression="JobResponsibility"/>
                                            <asp:BoundField DataField="PHKReason" HeaderText="Reason for Leaving" SortExpression="PHKReason"/>
                                            <asp:BoundField DataField="WorkPeriod" HeaderText="Period" SortExpression="WorkPeriod"/>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                                
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="PnlEditExp" runat="server" Visible="false">
                                    <table>
                                    <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoExp" runat="server" />
                                </td>
                                    </tr>
                                        <tr>
                                            <td>
                                                Company Name</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbcompanyName" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Type of Business</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <asp:DropDownList ID="ddlCompanyBusiness" runat="server" CssClass="DropDownList" 
                                                    Height="16px" Width="114px">
                                                    <asp:ListItem>Sekolah</asp:ListItem>
                                                    <asp:ListItem>Universitas</asp:ListItem>
                                                    <asp:ListItem>Akademi</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Delivery Addr 1</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbAddress1Addr" runat="server" CssClass="TextBox" MaxLength="100"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Delivery Addr 2</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbAddress2Addr" runat="server" CssClass="TextBox" MaxLength="100"
                                                    Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                City</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlCityExp" runat="server" CssClass="DropDownList" 
                                                    Height="16px" Width="114px">
                                                    <asp:ListItem>Pabrik</asp:ListItem>
                                                    <asp:ListItem>Puri</asp:ListItem>
                                                    <asp:ListItem>Office</asp:ListItem>
                                                    <asp:ListItem>Toko</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                Phone</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbPhoneExp" runat="server" CssClass="TextBox" MaxLength="30"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Job Title</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlJobTitleExp" runat="server" CssClass="DropDownList" 
                                                    Height="16px" Width="114px">
                                                 </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Dept / Division</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlDeptExp" runat="server" CssClass="DropDownList" 
                                                    Height="16px" Width="114px">
                                                 </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        
                                        <tr>
                                            <td>
                                                Salary</td>
                                            <td>
                                                :</td>
                                            <td>
                                               <asp:TextBox ID="tbsalaryExp" runat="server" CssClass="TextBox" MaxLength="16"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Job Description</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbJobDesc" runat="server" CssClass="TextBox" Width="136px" MaxLength="100"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Reason For Leaving</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbPHKReason" runat="server" CssClass="TextBox" Width="138px" MaxLength="50"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Period</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbWorkPeriod" runat="server" CssClass="TextBox" MaxLength="50"
                                                    Width="138px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: center">
                                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveExp" Text="Save" />									                                                                                                                                                        
                                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelExp" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                           
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                               </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabTraining" runat="server">
                <br />
                <asp:Panel ID="pnlTraining" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddT" Text="Add" />									                                                                                                                                                                                                                                
                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="Gridt" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditT" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                                                                        
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteT" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');" />									                                                                                                                                                                                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                               
                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="No"  SortExpression="ItemNo"/>
                                            <asp:BoundField DataField="TrainingCode" HeaderText="Training"  SortExpression="TrainingCode"/>
                                            <asp:BoundField DataField="TrainingName" HeaderText="Training Name"  SortExpression="TrainingName"/>
                                            <asp:BoundField DataField="TrainingPlace" HeaderText="Training Place"  SortExpression="TrainingPlace"/>
                                            <asp:BoundField DataField="TutorName" HeaderText="Instructor"  SortExpression="TutorName"/>
                                            <asp:BoundField DataField="Institution" HeaderText="Institution"  SortExpression="Institution"/>
                                            <asp:BoundField DataField="Location" HeaderText="Location"  SortExpression="Location"/>
                                            <asp:BoundField DataField="TrainingPeriod" HeaderText="Period"  SortExpression="TrainingPeriod"/>
                                            <asp:BoundField DataField="FgCertificate" HeaderText="Fg Certificate"  SortExpression="FgCertificate"/>
                                            <asp:BoundField DataField="Certificate" HeaderText="Certificate"  SortExpression="Certificate"/>
                                            <asp:BoundField DataField="CostType" HeaderText="Cost Type"  SortExpression="CostType"/>
                                            <asp:BoundField DataField="Nilai" HeaderText="Value" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"  SortExpression="Nilai"/>
                                            
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                                
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlEditTraining" runat="server" Visible="false">
                                    <table>
                                    <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoT" runat="server" />
                                </td>
                                    </tr>
                                        <tr>
                                            <td>
                                                Training Code</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTrainingCode" runat="server" AutoPostBack="true" 
                                                    CssClass="TextBox" ValidationGroup="Input" Width="80px" />
                                                <asp:Button ID="btnTraining" runat="server" class="btngo" Text="..." />
                                                <asp:Label ID="Label20" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Training Name</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTrainingName" runat="server" CssClass="TextBoxR" MaxLength="60"
                                                    Width="198px" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Training Place</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTrainingPlace" runat="server" CssClass="TextBox" MaxLength="10"
                                                    Width="198px" />
                                                <asp:Label ID="Label21" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Training Period</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTrainingPeriod" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Institution</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbInstitution" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="200px" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                Location</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbLocation" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Tutor Name</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTutorName" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cost Type</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlCostType" runat="server" CssClass="DropDownList" 
                                                    Width="110px">
                                                    <asp:ListItem>Company</asp:ListItem>
                                                    <asp:ListItem>Private</asp:ListItem>
                                                 </asp:DropDownList>
                                                <asp:Label ID="Label22" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                        
                                        <tr>
                                            <td>
                                                Value</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbNilai" runat="server" CssClass="TextBox" MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Certificate</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbCertificateT" runat="server" CssClass="TextBox" Width="136px" MaxLength="40"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Fg Certificate</td>
                                            <td>
                                                :</td>
                                            <td>
                                                 <asp:DropDownList ID="ddlFgCertificate" runat="server" CssClass="DropDownList" 
                                                    Width="80px">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                 </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="3" style="text-align: center">
                                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveT" Text="Save" />									                                                                                                                                                        
                                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelT" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                           
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                               </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabPrestasi" runat="server">
            <br>
                <asp:Panel runat="server" ID="PnlP">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddP" Text="Add" />									                                                                                                                                                        
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="GridP" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btneditP" Text="Edit" CommandNAme="edit" />									                                                                                                                                                                                            
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteP" Text="Delete" CommandNAme="delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                            
                                </ItemTemplate>
                          
                        </asp:TemplateField>
                        <asp:BoundField DataField="Tahun" HeaderText="Year" SortExpression="Tahun" />
                        <asp:BoundField DataField="Prestasi" HeaderText="Achievement" SortExpression="Prestasi"/>
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>      
       <asp:Panel runat="server" ID="PnlEditPrestasi" Visible="false">
            <table>
                <tr>
                    <td>Year</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbtahunP" runat="server" CssClass="TextBox" MaxLength="4"
                        Width="80px" />
                        <asp:Label ID="Label26" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td>Achievement</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbprestasi" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                        <asp:Label ID="Label25" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSavep" Text="Save" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelp" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />&nbsp;<br />
            <br />
       </asp:Panel> 
            </asp:View>
            
            <asp:View ID="TabBank" runat="server">
            <br>
                <asp:Panel runat="server" ID="PnlBank">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddb" Text="Add" />									                                                                                                                                                        
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="Gridb" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btneditb" Text="Edit" CommandNAme="edit" />									                                                                                                                                                                                            
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteb" Text="Delete" CommandNAme="delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                            
                                </ItemTemplate>
                          
                        </asp:TemplateField>
                        <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank" />
                        <asp:BoundField DataField="BankAddr1" HeaderText="Bank Address 1" SortExpression="BankAddr1"/>
                        <asp:BoundField DataField="BankAddr2" HeaderText="Bank Address 2" SortExpression="BankAddr2"/>
                        <asp:BoundField DataField="BankAccountNo" HeaderText="Bank Account" SortExpression="BankAccountNo"/>
                        <asp:BoundField DataField="BankAccountName" HeaderText="Bank Account Name" SortExpression="BankAccountName"/>
                        <asp:BoundField DataField="Banksandi" HeaderText="Bank Password" SortExpression="Banksandi"/>
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>      
       <asp:Panel runat="server" ID="pnlEditBank" Visible="false">
            <table>
                             <tr>
                                <td class="style1">
                                    Bank</td>
                                <td class="style2">
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlBank" runat="server" AutoPostBack="True" 
                                        CssClass="DropDownList" Width="199px" Height="16px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label23" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                                <td class="style4">
                                    &nbsp;</td>
                            </tr>

                <tr>
                    <td>Bank Address 1</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbBankAddr1" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                    </td>
                </tr>
                
                <tr>
                    <td>Bank Address 2</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbBankAddr2" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Bank Account</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbBankAccount" runat="server" CssClass="TextBox" MaxLength="30"
                        Width="198px" />
                        <asp:Label ID="Label24" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Bank Account Name</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbBankAccountName" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>Bank Password</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbsandi" runat="server" CssClass="TextBox" MaxLength="20"
                        Width="80px" />
                    </td>
                </tr>
                
                
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveb" Text="Save" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelb" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />
            
            <br />
            <br />
       </asp:Panel> 
            </asp:View>
            
            <asp:View ID="TabEmergency" runat="server">
               <br> 
                <asp:Panel ID="pnlEmergency" runat="server">
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddeme" Text="Add" CommandName = 'Insert'/>									                                
                   
                    <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                        <asp:GridView ID="Grideme" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditeme" Text="Edit" CommandNAme="Edit"  />									                                
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteeme" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                        </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="Banksandi"/>
                                <asp:BoundField DataField="ContactName" HeaderText="Contact Name" 
                                    SortExpression="ContactName" />
                                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender"/>
                                <asp:BoundField DataField="RelationShip" HeaderText="RelationShip" SortExpression="RelationShip"/>
                                <asp:BoundField DataField="Address1" HeaderText="Address 1" SortExpression="Address1"/>
                                <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/>
                                <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" SortExpression="ZipCode"/>
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"/>
                                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone"/>
                                <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax"/>
                                <asp:BoundField DataField="HandPhone" HeaderText="HP" SortExpression="HandPhone"/>
                                
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlEditEmergency" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoEmergency" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Name</td>
                                <td>
                                    :</td>
                                <td>
                                    
                                    <asp:TextBox ID="tbContactNameE" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="269px" />
                                    <asp:Label ID="Label27" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Gender</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGenderE" runat="server" CssClass="DropDownList" 
                                        Height="16px" Width="87px">
                                        <asp:ListItem>Female</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Relationship</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbRelationship" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="269px" />
                                    <asp:Label ID="Label28" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbEmailE" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="269px" />
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    Address 1</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbaddress1E" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 2</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbaddress2E" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Zip Code</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbZipCodeE" runat="server" CssClass="TextBox" MaxLength="10"
                                        Width="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbPhoneE" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fax</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbFax" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="109px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    HP</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbHP" runat="server" CssClass="TextBox" MaxLength="30"
                                        Width="109px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveeme" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btncanceleme" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />
                 </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabSociety" runat="server">
                <br />
                <asp:Panel ID="pnlSociety" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddso" Text="Add" />									                                
                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="Gridso" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditso" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                                                                        
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteso" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');" />									                                                                                                                                                                                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                               
                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="ItemNo"/>
                                            <asp:BoundField DataField="SocietyName" HeaderText="Society" SortExpression="SocietyName"/>
                                            <asp:BoundField DataField="JobInfo" HeaderText="Job Info" SortExpression="JobInfo"/>
                                            <asp:BoundField DataField="Jobtitle" HeaderText="Job Title" SortExpression="Jobtitle"/>
                                            <asp:BoundField DataField="OccupationYear" HeaderText="Year" SortExpression="OccupationYear"/>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                                
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlEditSociety" runat="server" Visible="false">
                                    <table>
                                    <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNoS" runat="server" />
                                </td>
                                    </tr>
                                        <tr>
                                            <td>
                                                Society</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbSociety" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="198px" />
                                                <asp:Label ID="Label29" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                Job Info</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbJobInfo" runat="server" CssClass="TextBox" MaxLength="100"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Job Title</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbJobTitle" runat="server" CssClass="TextBox" MaxLength="60"
                                                    Width="200px" />
                                            </td>
                                        </tr>
                                        
                                        
                                        <tr>
                                            <td>
                                                Year</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbYear" runat="server" CssClass="TextBox" MaxLength="30"
                                                    Width="198px" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="3" style="text-align: center">
                                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveso" Text="Save" />									                                                                                                                                                        
                                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelso" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                           
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                               </asp:Panel>
            </asp:View>
            
            <asp:View ID="TabHobby" runat="server">
            <br>
                <asp:Panel runat="server" ID="pnlHobby">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddh" Text="Add" />									                                                                                                                                                        
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="Gridh" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnedith" Text="Edit" CommandNAme="edit" />									                                                                                                                                                                                            
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteh" Text="Delete" CommandNAme="delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                            
                                </ItemTemplate>
                           
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderText="No" SortExpression="ItemNo" />
                        <asp:BoundField DataField="Hobby" HeaderText="Hobby" SortExpression="Hobby" />
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>      
       <asp:Panel runat="server" ID="pnlEditHobby" Visible="false">
            <table>
                 <tr>
                      <td>Item No</td>
                      <td>:</td>
                      <td><asp:Label ID="lbItemNoHobby" runat="server" /></td>
                 </tr>
                <tr>
                    <td>Hobby</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbHobby" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                        <asp:Label ID="Label30" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsaveH" Text="Save" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelH" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
       </asp:Panel> 
            </asp:View>
            
            <asp:View ID="TabMemo" runat="server">
            <br>
                <asp:Panel runat="server" ID="pnlMemo">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnaddM" Text="Add" />									                                                                                                                                                        
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="GridM" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btneditm" Text="Edit" CommandNAme="edit" />									                                                                                                                                                                                            
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeletem" Text="Delete" CommandNAme="delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                            
                                </ItemTemplate>
                           
                        </asp:TemplateField>
                        <asp:BoundField DataField="ReferenceBy" HeaderText="Reference By" SortExpression="ReferenceBy" />
                        <asp:BoundField DataField="ReferenceAddr1" HeaderText="Reference Addr1" SortExpression="ReferenceAddr1"/>
                        <asp:BoundField DataField="ReferenceAddr2" HeaderText="Reference Addr2" SortExpression="ReferenceAddr2"/>
                        <asp:BoundField DataField="ReferenceJob" HeaderText="Reference Job" SortExpression="ReferenceJob"/>
                        <asp:BoundField DataField="ReferencePhone" HeaderText="Reference Phone" SortExpression="ReferencePhone"/>
                        <asp:BoundField DataField="Memo1" HeaderText="Memo" SortExpression="Memo1"/>
                        <asp:BoundField DataField="Memo2" HeaderText="Memo 2" SortExpression="Memo2"/>
                        <asp:BoundField DataField="Memo3" HeaderText="Memo 3" SortExpression="Memo3"/>
                       
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>      
       <asp:Panel runat="server" ID="pnlEditMemo" Visible="false">
            <table>
                <tr>
                    <td>Reference By</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbReferenceBy" runat="server" CssClass="TextBox" MaxLength="60"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Reference Address</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbReferenceAddr1" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Reference Address 2</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbReferenceAddr2" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Reference Job</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbReferenceJob" runat="server" CssClass="TextBox" MaxLength="100"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Reference Phone</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbReferencePhone" runat="server" CssClass="TextBox" MaxLength="30"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Memo</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbMemo" runat="server" CssClass="TextBox" MaxLength="255"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Memo 2</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbMemo2" runat="server" CssClass="TextBox" MaxLength="255"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Memo 3</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbMemo3" runat="server" CssClass="TextBox" MaxLength="255"
                        Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsavem" Text="Save" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelm" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
       </asp:Panel> 
            </asp:View>
            
        </asp:MultiView>
      </asp:Panel> 
      <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
      <br />            
    </div>
   
    
   
    </form>
    </body>
</html>
