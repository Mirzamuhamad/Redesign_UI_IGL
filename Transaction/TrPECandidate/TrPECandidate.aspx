<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPECandidate.aspx.vb" Inherits="Transaction_TrPECandidate_TrPECandidate" %>

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
       
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbHeight").value = setdigit(document.getElementById("tbHeight").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbWeight").value = setdigit(document.getElementById("tbWeight").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
                                                
            
        }catch (err){
            alert(err.description);
          }      
        }   
        
   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Candidate Data</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Candidate No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Candidate Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                    <asp:ListItem Value="JobTitle">JobTitle Code</asp:ListItem>
                    <asp:ListItem Value="JobTitle_Name">JobTitle Name</asp:ListItem>                                        
                    <asp:ListItem Value="EmpStatus">EmpStatus Code</asp:ListItem>                    
                    <asp:ListItem Value="EmpStatus_Name">EmpStatus Name</asp:ListItem>
                    <asp:ListItem Value="Source">Source</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Gender">Gender</asp:ListItem>
                    <asp:ListItem Value="BirthPlace">Birth Place</asp:ListItem>
                    <asp:ListItem Value="BirthDate">Birth Date</asp:ListItem>
                    <asp:ListItem Value="TypeCard">Type Card</asp:ListItem>
                    <asp:ListItem Value="IDCard">ID Card</asp:ListItem>
                    <asp:ListItem Value="Source">Source</asp:ListItem>
                    <asp:ListItem Value="Religion">Religion</asp:ListItem>
                    <asp:ListItem Value="Weight">Weight</asp:ListItem>
                    <asp:ListItem Value="Height">Height</asp:ListItem>
                    <asp:ListItem Value="MaritalSt">Marital Status</asp:ListItem>
                    <asp:ListItem Value="HandPhone">HandPhone</asp:ListItem>
                    <asp:ListItem Value="Email">Email</asp:ListItem>
                    <asp:ListItem Value="ResAddr1">Res. Addr1</asp:ListItem>
                    <asp:ListItem Value="ResAddr2">Res. Addr2</asp:ListItem>
                    <asp:ListItem Value="ResZipCode">Res. ZipCode</asp:ListItem>
                    <asp:ListItem Value="ResCity">Res. City</asp:ListItem>
                    <asp:ListItem Value="ResPhone">Res. Phone</asp:ListItem>
                    <asp:ListItem Value="OriAddr1">Ori. Addr1</asp:ListItem>
                    <asp:ListItem Value="OriAddr2">Ori. Addr2</asp:ListItem>
                    <asp:ListItem Value="OriZipCode">Ori. ZipCode</asp:ListItem>
                    <asp:ListItem Value="OriCity">Ori. City</asp:ListItem>
                    <asp:ListItem Value="OriPhone">Ori. Phone</asp:ListItem>
                    <asp:ListItem Value="Penyakit">Sickness</asp:ListItem>
                    <asp:ListItem Value="EmpReference">Emp. Reference</asp:ListItem>
                    <asp:ListItem Value="UserPrep">User Prep</asp:ListItem>
                    <asp:ListItem Value="DatePrep">Date Prep</asp:ListItem>
                    <asp:ListItem Value="UserAppr">User Appr</asp:ListItem>
                    <asp:ListItem Value="DateAppr">Date Appr</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Candidate No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Candidate Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                    <asp:ListItem Value="JobTitle">JobTitle Code</asp:ListItem>
                    <asp:ListItem Value="JobTitle_Name">JobTitle Name</asp:ListItem>                                        
                    <asp:ListItem Value="EmpStatus">EmpStatus Code</asp:ListItem>                    
                    <asp:ListItem Value="EmpStatus_Name">EmpStatus Name</asp:ListItem>
                    <asp:ListItem Value="Source">Source</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Gender">Gender</asp:ListItem>
                    <asp:ListItem Value="BirthPlace">Birth Place</asp:ListItem>
                    <asp:ListItem Value="BirthDate">Birth Date</asp:ListItem>
                    <asp:ListItem Value="TypeCard">Type Card</asp:ListItem>
                    <asp:ListItem Value="IDCard">ID Card</asp:ListItem>
                    <asp:ListItem Value="Source">Source</asp:ListItem>
                    <asp:ListItem Value="Religion">Religion</asp:ListItem>
                    <asp:ListItem Value="Weight">Weight</asp:ListItem>
                    <asp:ListItem Value="Height">Height</asp:ListItem>
                    <asp:ListItem Value="MaritalSt">Marital Status</asp:ListItem>
                    <asp:ListItem Value="HandPhone">HandPhone</asp:ListItem>
                    <asp:ListItem Value="Email">Email</asp:ListItem>
                    <asp:ListItem Value="ResAddr1">Res. Addr1</asp:ListItem>
                    <asp:ListItem Value="ResAddr2">Res. Addr2</asp:ListItem>
                    <asp:ListItem Value="ResZipCode">Res. ZipCode</asp:ListItem>
                    <asp:ListItem Value="ResCity">Res. City</asp:ListItem>
                    <asp:ListItem Value="ResPhone">Res. Phone</asp:ListItem>
                    <asp:ListItem Value="OriAddr1">Ori. Addr1</asp:ListItem>
                    <asp:ListItem Value="OriAddr2">Ori. Addr2</asp:ListItem>
                    <asp:ListItem Value="OriZipCode">Ori. ZipCode</asp:ListItem>
                    <asp:ListItem Value="OriCity">Ori. City</asp:ListItem>
                    <asp:ListItem Value="OriPhone">Ori. Phone</asp:ListItem>
                    <asp:ListItem Value="Penyakit">Sickness</asp:ListItem>
                    <asp:ListItem Value="EmpReference">Emp. Reference</asp:ListItem>
                    <asp:ListItem Value="UserPrep">User Prep</asp:ListItem>
                    <asp:ListItem Value="DatePrep">Date Prep</asp:ListItem>
                    <asp:ListItem Value="UserAppr">User Appr</asp:ListItem>
                    <asp:ListItem Value="DateAppr">Date Appr</asp:ListItem>                              
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
                  <asp:BoundField DataField="RequestNo" HeaderStyle-Width="80px" SortExpression="RequestNo" HeaderText="Request No"></asp:BoundField>
                  <asp:BoundField DataField="JobTitle" HeaderStyle-Width="80px" SortExpression="JobTitle" HeaderText="JobTitle Code"></asp:BoundField>
                  <asp:BoundField DataField="JobTitle_Name" HeaderStyle-Width="200px" SortExpression="JobTitle_Name" HeaderText="JobTitle Name"></asp:BoundField>
                  <asp:BoundField DataField="EmpStatus" HeaderStyle-Width="80px" SortExpression="EmpStatus" HeaderText="EmpStatus Code"></asp:BoundField>
                  <asp:BoundField DataField="EmpStatus_Name" HeaderStyle-Width="200px" SortExpression="EmpStatus_Name" HeaderText="EmpStatus Name"></asp:BoundField>
                  <asp:BoundField DataField="Source" HeaderStyle-Width="80px" SortExpression="Source" HeaderText="Source"></asp:BoundField>
                  <asp:BoundField DataField="EmpName" HeaderStyle-Width="250px" SortExpression="EmpName" HeaderText="Emp Name"></asp:BoundField>
                  <asp:BoundField DataField="Gender" HeaderStyle-Width="80px" SortExpression="Gender" HeaderText="Gender"></asp:BoundField>
                  <asp:BoundField DataField="BirthPlace" HeaderStyle-Width="250px" SortExpression="BirthPlace" HeaderText="Birth Place"></asp:BoundField>
                  <asp:BoundField DataField="BirthDate" HeaderStyle-Width="80px" SortExpression="BirthDate" HeaderText="Birth Date"></asp:BoundField>
                  <asp:BoundField DataField="TypeCard" HeaderStyle-Width="250px" SortExpression="TypeCard" HeaderText="Type Card"></asp:BoundField>
                  <asp:BoundField DataField="IDCard" HeaderStyle-Width="80px" SortExpression="IDCard" HeaderText="ID Card"></asp:BoundField>
                  <asp:BoundField DataField="Religion" HeaderStyle-Width="250px" SortExpression="Religion" HeaderText="Religion"></asp:BoundField>
                  <asp:BoundField DataField="Weight" HeaderStyle-Width="80px" SortExpression="Weight" HeaderText="Weight"></asp:BoundField>
                  <asp:BoundField DataField="Height" HeaderStyle-Width="250px" SortExpression="Height" HeaderText="Height"></asp:BoundField>
                  <asp:BoundField DataField="MaritalSt" HeaderStyle-Width="80px" SortExpression="MaritalSt" HeaderText="MaritalSt"></asp:BoundField>
                  <asp:BoundField DataField="HandPhone" HeaderStyle-Width="250px" SortExpression="HandPhone" HeaderText="HandPhone"></asp:BoundField>
                  <asp:BoundField DataField="Email" HeaderStyle-Width="80px" SortExpression="Email" HeaderText="Email"></asp:BoundField>
                  <asp:BoundField DataField="ResAddr1" HeaderStyle-Width="250px" SortExpression="ResAddr1" HeaderText="Res. Addr1"></asp:BoundField>
                  <asp:BoundField DataField="ResAddr2" HeaderStyle-Width="250px" SortExpression="ResAddr2" HeaderText="Res. Addr2"></asp:BoundField>
                  <asp:BoundField DataField="ResZipCode" HeaderStyle-Width="250px" SortExpression="ResZipCode" HeaderText="Res. ZipCode"></asp:BoundField>
                  <asp:BoundField DataField="ResCity" HeaderStyle-Width="250px" SortExpression="ResCity" HeaderText="Res. City"></asp:BoundField>
                  <asp:BoundField DataField="ResPhone" HeaderStyle-Width="250px" SortExpression="ResPhone" HeaderText="Res. Phone"></asp:BoundField>
                  <asp:BoundField DataField="OriAddr1" HeaderStyle-Width="250px" SortExpression="OriAddr1" HeaderText="Ori. Addr1"></asp:BoundField>
                  <asp:BoundField DataField="OriAddr2" HeaderStyle-Width="250px" SortExpression="OriAddr2" HeaderText="Ori. Addr2"></asp:BoundField>
                  <asp:BoundField DataField="OriZipCode" HeaderStyle-Width="250px" SortExpression="OriZipCode" HeaderText="Ori. ZipCode"></asp:BoundField>
                  <asp:BoundField DataField="OriCity" HeaderStyle-Width="250px" SortExpression="OriCity" HeaderText="Ori. City"></asp:BoundField>
                  <asp:BoundField DataField="OriPhone" HeaderStyle-Width="250px" SortExpression="OriPhone" HeaderText="Ori. Phone"></asp:BoundField>
                  <asp:BoundField DataField="Penyakit" HeaderStyle-Width="250px" SortExpression="Penyakit" HeaderText="Sickness"></asp:BoundField>
                  <asp:BoundField DataField="EmpReference" HeaderStyle-Width="250px" SortExpression="EmpReference" HeaderText="Emp. Reference"></asp:BoundField>
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
            <td>Candidate No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
        
            <td>Candidate Date</td>
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
            <td>Request No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbRequestNo" MaxLength="20" Width="150px" />
                <asp:Button Class="btngo" ID="btnRequestNo" Text="..." runat="server" ValidationGroup="Input" />
            </td> 
            <td>Source</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSource" MaxLength="60" CssClass="TextBox" Width="225px"/>
            </td>            
        </tr>
        
        <tr> 
             <td>Emp. Status</td>
             <td>:</td>
             <td colspan="">                            
                 <asp:DropDownList CssClass="DropDownList" ID="ddlEmpStatus" runat="server" Enabled ="false" Width="180px"/>
             </td>     
             <td>Job Title</td>
             <td>:</td>
             <td>                            
                 <asp:DropDownList CssClass="DropDownList" ID="ddlJobTitle" runat="server" Enabled ="false" Width="180px"/>
             </td>     
        </tr>
        <tr>
            <td>Emp. Name</td>
            <td>:</td>
            <td style="width:300px;"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbEmpName" MaxLength="60" CssClass="TextBox" Width="250px"/></td>
            <td>Gender</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlGender" runat="server" >
                        <asp:ListItem Selected="True">Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                    </asp:DropDownList> 
            </td>  
        </tr>
        
        <tr>
            <td>Birth Place</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbBirthPlace" MaxLength="60" Width="180px"/></td>
        
            <td>Birth Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbBirthDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr> 
             <td>Religion</td>
             <td>:</td>
             <td>                            
                 <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" ID="ddlReligion" runat="server"/>
             </td> 
             <td>Marital Status</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlMaritalSt" runat="server" >
                        <asp:ListItem Selected="True">Single</asp:ListItem>
                        <asp:ListItem>Married</asp:ListItem>
                    </asp:DropDownList> 
            </td>                          
        </tr>
        <tr>
            <td>Height & Weight</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbHeight" MaxLength="20" Width="50px"/> cm                    
                <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input" ID="tbWeight" MaxLength="20" Width="50px"/> Kgs
            </td>       
        
            <td>Type & ID Card</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlTypeCard" runat="server" >
                        <asp:ListItem Selected="True">KTP</asp:ListItem>
                        <asp:ListItem>SIM</asp:ListItem>
                        <asp:ListItem>Kartu Pelajar</asp:ListItem>
                        <asp:ListItem>Passport</asp:ListItem>
                 </asp:DropDownList>       
                <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbIDCard" MaxLength="30" Width="150px"/>
            </td>   
        </tr>
        <tr>
            <td>HP No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbHandPhone" MaxLength="30" Width="150px"/></td>        
            <td>Email</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbEmail" MaxLength="60" Width="250px"/></td>       
        </tr>
        <tr>
            <td>Sickness</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPenyakit" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" Width="300px"/></td>
        </tr>
        <tr>
            <td>Reference By</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmpReference" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbEmpReferenceName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnEmpReference" Text="..." runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="font-size:medium; text-align:center; font-style:italic; text-decoration: underline;"><strong>Resident</strong></td>            
            <td colspan="3" style="font-size:medium; text-align:center; font-style:italic; text-decoration: underline;"><strong>Original</strong></td>            
        </tr>            
        <tr>
            <td>Address #1</td>
            <td>:</td>
            <td>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbResAddr1" MaxLength="60" CssClass="TextBox" Width="280px"/>                                
            </td>     
            <td>Address #1</td>
            <td>:</td>             
            <td>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbOriAddr1" MaxLength="60" CssClass="TextBox" Width="280px"/>                                
            </td>                    
        </tr>
        <tr>
            <td>Address #2</td>
            <td>:</td>
            <td>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbResAddr2" MaxLength="60" CssClass="TextBox" Width="280px"/>                
            </td>     
            <td>Address #2</td>
            <td>:</td>             
            <td>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbOriAddr2" MaxLength="60" CssClass="TextBox" Width="280px"/>
            </td>                    
        </tr>
        <tr>
            <td>City & Zip Code</td>
            <td>:</td>
            <td>                            
                 <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input"  ID="ddlResCity" runat="server" Width="200px"/>
                 <asp:TextBox CssClass="TextBox" runat="server"  ValidationGroup="Input"  ID="tbResZipCode" MaxLength="10" Width="80px"/>
             </td>   
             <td>City & Zip Code</td>
            <td>:</td>
             <td>                            
                 <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input"  ID="ddlOriCity" runat="server" Width="180px"/>
                 <asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbOriZipCode" MaxLength="10" Width="80px"/>
             </td>   
        </tr>
        
        
        <tr>
            <td>Phone</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbResPhone" MaxLength="30" Width="180px"/></td>   
            <td>Phone</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  ID="tbOriPhone" MaxLength="30" Width="180px"/>
            </td>   
        </tr>        
      </table>  
      
      <br />      
      <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail Family" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Education" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Detail Experience" Value="2"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />	
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
                            <asp:BoundField DataField="FamilyType" HeaderStyle-Width="100px" HeaderText="Family Type" />
                            <asp:BoundField DataField="FamilyName" HeaderStyle-Width="150px" HeaderText="Family Name" />
                            <asp:BoundField DataField="BirthPlace" HeaderStyle-Width="100px" HeaderText="Birth Place" />
                            <asp:BoundField DataField="BirthDate" HeaderStyle-Width="100px" HeaderText="Birth Date" />                            
                            <asp:BoundField DataField="Gender" HeaderText="Gender" />
                            <asp:BoundField DataField="EduLevel" HeaderStyle-Width="100px" HeaderText="Education Level" />
                            <asp:BoundField DataField="Occupation" HeaderStyle-Width="100px" HeaderText="Occupation" />
                            <asp:BoundField DataField="Address1" HeaderStyle-Width="100px" HeaderText="Address1" />   
                            <asp:BoundField DataField="Address2" HeaderStyle-Width="100px" HeaderText="Address2" />   
                            <asp:BoundField DataField="Phone" HeaderStyle-Width="80px" HeaderText="Phone" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table> 
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td>           
                    </tr> 
                    <tr>
                        <td>Family Type</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlFamilyType" runat="server" >
                                    <asp:ListItem Selected="True">Spouse</asp:ListItem>
                                    <asp:ListItem>Children</asp:ListItem>
                                    <asp:ListItem>Parent</asp:ListItem>
                                </asp:DropDownList> 
                        </td> 
                        <td>Family Name</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFamilyName" MaxLength="60" Width="250px"/>                                                       
                        </td>
                    </tr>             
                    
                    <tr>
                        <td>Birth Place</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbFamBirthPlace" MaxLength="60"  Width="180px"/></td>
                    
                        <td>Birth Date</td>
                        <td>:</td>
                        <td>
                        <BDP:BasicDatePicker ID="tbFamBirthDate" runat="server" DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                        </td>            
                    </tr> 
                    <tr>
                        <td>Gender</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlFamGender" runat="server" >
                                    <asp:ListItem Selected="True">Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                </asp:DropDownList> 
                        </td> 
                        <td>Education Level</td>
                        <td>:</td>
                        <td>                            
                            <asp:DropDownList CssClass="DropDownList" ID="ddlFamEduLevel" runat="server"/>
                        </td>  
                    </tr>
                    <tr>
                        <td>Address #1</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbFamAddr1" MaxLength="60" CssClass="TextBox" Width="250px"/>                
                            
                        </td>           
                        <td>Address #2</td>
                        <td>:</td>                        
                        <td>       
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbFamAddr2" MaxLength="60" CssClass="TextBox" Width="250px"/>
                        </td>
                    </tr>          
                    <tr>
                        <td>Occupation</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbFamOccupation" MaxLength="50" Width="200px"/></td>   
                        <td>Phone</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbFamPhone" MaxLength="30" Width="160px"/></td>   
                    </tr>          
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	              
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
   							       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                </ItemTemplate>
                                <EditItemTemplate>
                               	    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                </EditItemTemplate>
                            </asp:TemplateField>                                      
                            
                            <asp:BoundField DataField="EduLevel" HeaderStyle-Width="150px" HeaderText="Education Level"  />
                            <asp:BoundField DataField="SchoolName" HeaderStyle-Width="150px" HeaderText="School Name" />
                            <asp:BoundField DataField="EduCity" HeaderStyle-Width="80px" HeaderText="Education City" />
                            <asp:BoundField DataField="EduMajor" HeaderStyle-Width="100px" HeaderText="Education Major" />
                            <asp:BoundField DataField="CertificateNo" HeaderStyle-Width="100px" HeaderText="Certificate No" />
                            <asp:BoundField DataField="DurasiYear" HeaderText="Durasi Year" />
                            <asp:BoundField DataField="GPA" HeaderText="GPA" />
                            <asp:BoundField DataField="FgGraduated" HeaderText="Fg Graduated" />
                            <asp:BoundField DataField="Graduated" HeaderStyle-Width="80px" HeaderText="Year Graduated" />
                                                       
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>                    
                        <td>Education Level</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlEduLevel" runat="server"/></td>  
                        <td>School Name</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSchoolName" Width="250px" MaxLength="60" /></td>
                    </tr>   
                    
                    <tr>                    
                        
                    </tr>  
                    <tr>
                        <td>Education City</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlEduCity" runat="server"/></td>                           
                        <td>Education Major</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEduMajor" Width="250px"  MaxLength="60" /></td>
                    </tr>
                    <tr>                    
                        
                    </tr>
                     <tr>                    
                        <td>Certificate No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCertificateNo" Width="250px" MaxLength="60" /></td>
                    </tr>    
                    <tr>                    
                        <td>Durasi Year</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbDurasiYear" MaxLength="2" /></td>
                        <td>GPA</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbGPA" MaxLength="5" /></td>
                    </tr>  
                    <tr>                    
                        
                    </tr> 
                    <tr>   
                        <td>Fg Graduated</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgGraduate" runat="server" >
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList> 
                        </td>                  
                        <td>Graduated Year</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbGraduateYear" MaxLength="5" /></td>
                    </tr>
                                           
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									


           </asp:Panel> 
               
            </asp:View>    
            <asp:View ID="Tab3" runat="server">
                <asp:Panel ID="pnlDt3" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />	              
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
   							       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                </ItemTemplate>
                                <EditItemTemplate>
                               	    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                </EditItemTemplate>
                            </asp:TemplateField>                                      
                            
                            <asp:BoundField DataField="ItemNo" HeaderText="No"  />
                            <asp:BoundField DataField="CompanyName" HeaderStyle-Width="150px" HeaderText="Company Name" />
                            <asp:BoundField DataField="CompanyBusiness" HeaderStyle-Width="80px" HeaderText="Company Business" />
                            <asp:BoundField DataField="WorkPeriod" HeaderStyle-Width="80px" HeaderText="Work Period" />
                            <asp:BoundField DataField="Address1" HeaderStyle-Width="80px" HeaderText="Address 1" />
                            <asp:BoundField DataField="Address2" HeaderStyle-Width="120px" HeaderText="Address 2" />
                            <asp:BoundField DataField="CompanyCity" HeaderText="Company City" />
                            <asp:BoundField DataField="CompanyPhone" HeaderText="Company Phone" />
                            <asp:BoundField DataField="JobTitle" HeaderStyle-Width="80px" HeaderText="Job Title" />
                            <asp:BoundField DataField="Department" HeaderStyle-Width="80px" HeaderText="Department" />
                            <asp:BoundField DataField="JobResponsibilty" HeaderStyle-Width="120px" HeaderText="Job Responsibilty" />
                            <asp:BoundField DataField="CurrCode" HeaderText="Curr" />
                            <asp:BoundField DataField="LastSalary" HeaderText="Last Salary" />
                            <asp:BoundField DataField="PHKReason" HeaderStyle-Width="80px" HeaderText="PHK Reason" />
                                                       
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />	              
   
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
                        <td>Company Name</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbCompanyName" MaxLength="60" Width="250px"/></td>
                        <td>Company Business</td>
                        <td>:</td>
                        <td width="100px"><asp:TextBox CssClass="TextBox" runat="server" ID="tbCompanyBusiness" MaxLength="60" Width="250px"/></td> 
                    </tr>                     
                     <tr>
                        <td>Address #1</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbCompanyAddr1" MaxLength="60" CssClass="TextBox" Width="250px"/>                                            
                        </td>           
                        <td>Address #2</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbCompanyAddr2" MaxLength="60" CssClass="TextBox" Width="250px"/>
                        </td>       
                    </tr>
                    <tr>
                        <td>City</td>
                        <td>:</td>
                        <td>                            
                             <asp:DropDownList CssClass="DropDownList" ID="ddlCompanyCity" runat="server"/>
                        </td>   
                        <td>Last Salary</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbLastSalary" MaxLength="20" Width="100px"/>
                            <asp:DropDownList CssClass="DropDownList" ID="ddlCurr" runat="server" Width="60px"/>
                        </td>   
                    </tr>
                    <tr>
                                           
                        <td>TelePhone</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbCompanyPhone" MaxLength="30"  Width="150px"/></td>   
                        <td>Job Title</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbJobTitle" MaxLength="50" Width="157px"/></td>                         
                    </tr>
                    <tr>
                        
                    </tr>                      
                    <tr>
                        <td>Department</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDepartment" MaxLength="50" Width="250px"/></td>                         
                        <td>Job Responsibilty</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbJobResponsibilty" MaxLength="100" Width="250px"/></td>                         
                    </tr>  
                    <tr>
                        
                    </tr>                      
                    <tr>
                        <td>Work Period</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbWorkPeriod" MaxLength="50" Width="157px"/></td>                         
                         <td>PHK Reason</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPHKReason" MaxLength="50" Width="157px"/></td>                         
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
