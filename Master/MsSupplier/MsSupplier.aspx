<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSupplier.aspx.vb" Inherits="Master_MsSupplier_MsSupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
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
    
    
</script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
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
            width: 137px;
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
            width: 75px;
        }
        .style7
        {
            width: 94px;
            height: 17px;
        }
        .style8
        {
            width: 8px;
            height: 17px;
        }
        .style9
        {
            height: 17px;
        }
        .style10
        {
            width: 13px;
            height: 17px;
        }
        .style11
        {
            width: 75px;
            height: 17px;
        }
        .style12
        {
            width: 9px;
            height: 17px;
        }
        .style13
        {
            width: 137px;
            height: 17px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Supplier File</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="SuppCode">Supplier Code</asp:ListItem>
                      <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="MemberOfGroup">Member Of Group</asp:ListItem>                        
                      <asp:ListItem Value="SisterCompany">Sister Company</asp:ListItem>
                      <asp:ListItem Value="SuppClassName">Supplier Class</asp:ListItem>
                      <asp:ListItem Value="SuppTypeName">Supplier Type</asp:ListItem>
                      <asp:ListItem Value="Address1">Address1</asp:ListItem>
                      <asp:ListItem Value="Address2">Address2</asp:ListItem>
                      <asp:ListItem Value="CityName">City</asp:ListItem>
                      <asp:ListItem Value="ZipCode">Zip Code</asp:ListItem>
                      <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                      <asp:ListItem Value="Fax">Fax</asp:ListItem>
                      <asp:ListItem Value="Email">Email</asp:ListItem>
                      <asp:ListItem Value="CurrCode">Currency</asp:ListItem>
                      <asp:ListItem Value="TermName">Term</asp:ListItem>
                      <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                      <asp:ListItem Value="FgPPN">PPN</asp:ListItem>
                      <asp:ListItem Value="NPPKP">PKKP</asp:ListItem>
                      <asp:ListItem Value="OwnerName">Director Name</asp:ListItem>
                      <asp:ListItem Value="ContactPerson">Contact Person</asp:ListItem>
                      <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                      <asp:ListItem Value="ContactHp">Contact Phone</asp:ListItem>                 
                      <asp:ListItem Value="ISOCertNo">ISO Certificate No</asp:ListItem>
                      <asp:ListItem Value="SIUPNo">SIUP No</asp:ListItem>
                      <asp:ListItem Value="FgSubkon">Supplier Subcont</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse Subcont</asp:ListItem>
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="Button1" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/> 
                  <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />
            </td>
            <td>&nbsp;</td>
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
                  <asp:ListItem Selected="True" Value="SuppCode">Supplier Code</asp:ListItem>
                  <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
                  <asp:ListItem Value="MemberOfGroup">Member Of Group</asp:ListItem>                        
                  <asp:ListItem Value="SisterCompany">Sister Company</asp:ListItem>
                  <asp:ListItem Value="SuppClassName">Supplier Class</asp:ListItem>
                  <asp:ListItem Value="SuppTypeName">Supplier Type</asp:ListItem>
                  <asp:ListItem Value="Address1">Address1</asp:ListItem>
                  <asp:ListItem Value="Address2">Address2</asp:ListItem>
                  <asp:ListItem Value="CityName">City</asp:ListItem>
                  <asp:ListItem Value="ZipCode">Zip Code</asp:ListItem>
                  <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                  <asp:ListItem Value="Fax">Fax</asp:ListItem>
                  <asp:ListItem Value="Email">Email</asp:ListItem>
                  <asp:ListItem Value="CurrCode">Currency</asp:ListItem>
                  <asp:ListItem Value="TermName">Term</asp:ListItem>
                  <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                  <asp:ListItem Value="FgPPN">PPN</asp:ListItem>
                  <asp:ListItem Value="NPPKP">PKKP</asp:ListItem>
                  <asp:ListItem Value="OwnerName">Director Name</asp:ListItem>
                  <asp:ListItem Value="ContactPerson">Contact Person</asp:ListItem>
                  <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                  <asp:ListItem Value="ContactHp">Contact Phone</asp:ListItem>                 
                  <asp:ListItem Value="ISOCertNo">ISO Certificate No</asp:ListItem>
                  <asp:ListItem Value="SIUPNo">SIUP No</asp:ListItem>
                  <asp:ListItem Value="FgSubkon">Supplier Subcont</asp:ListItem>
                  <asp:ListItem Value="Wrhs_Name">Warehouse Subcont</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add"/>&nbsp &nbsp &nbsp
            <br />
          <div style="border:0px  solid; width:100%; height:350px; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
              <asp:TemplateField Visible="false">
                  <ItemTemplate><asp:Label ID="cbSelectHd" runat="server" text='' /></ItemTemplate>
              </asp:TemplateField>
              
              <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                  <ItemTemplate>
                   <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                   <asp:ListItem Selected="True" Text="View" />
                   <asp:ListItem Text="Edit" />
                   <%--<asp:ListItem Text="Print" />--%>
                   <asp:ListItem>Delete</asp:ListItem>                   
                   </asp:DropDownList>
              <asp:Button class="btngo" runat="server" ID="btnExpand" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
              </ItemTemplate>
              <HeaderStyle Width="110px" />
              </asp:TemplateField>
              
                  <asp:BoundField DataField="SuppCode" SortExpression="SuppCode" HeaderText="Supplier Code"></asp:BoundField>
                  <asp:BoundField DataField="SuppName" HeaderText="Supplier Name"></asp:BoundField>
                  <asp:BoundField DataField="MemberOfGroup" SortExpression="MemberOfGroup" HeaderText="Member Of Group"></asp:BoundField>
                  <asp:BoundField DataField="SisterCompany" SortExpression="SisterCompany" HeaderText="Sister Company"></asp:BoundField>
                  <asp:BoundField DataField="SuppClass" SortExpression="SuppClass" HeaderText="Supplier Class Code"></asp:BoundField>
                  <asp:BoundField DataField="SuppClassName" SortExpression="SuppClassName" HeaderText="Supplier Class"></asp:BoundField>
                  <asp:BoundField DataField="SuppType" SortExpression="SuppType" HeaderText="Supplier Type Code"></asp:BoundField>    
                  <asp:BoundField DataField="SuppTypeName" SortExpression="SuppTypeName" HeaderText="Supplier Type"></asp:BoundField>
                  <asp:BoundField DataField="Address1" HeaderText="Address1" />
                  <asp:BoundField DataField="Address2" HeaderText="Address2" />
                  <asp:BoundField DataField="City" SortExpression="City" HeaderText="City Code"></asp:BoundField>
                  <asp:BoundField DataField="CityName" SortExpression="CityName" HeaderText="City"></asp:BoundField>
                  <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" />
                  <asp:BoundField DataField="Phone" HeaderText="Telephone" />
                  <asp:BoundField DataField="Fax" HeaderText="Fax" />
                  <asp:BoundField DataField="Email" HeaderText="Email"  />
                  <asp:BoundField DataField="CurrCode" HeaderText="Currency" />
                  <asp:BoundField DataField="Term" SortExpression="Term" HeaderText="Term Code"></asp:BoundField>
                  <asp:BoundField DataField="TermName" SortExpression="TermName"  HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="NPWP" HeaderText="NPWP" />
                  <asp:BoundField DataField="fgppn" HeaderText="PPN" />
                  <asp:BoundField DataField="nppkp" HeaderText="PKKP" />
                  <asp:BoundField DataField="OwnerName" HeaderText="Director Name" />
                  <asp:BoundField DataField="ContactPerson" HeaderText="ContactPerson"></asp:BoundField>
                  <asp:BoundField DataField="ContactTitle" HeaderText="Contact Title" />
                  <asp:BoundField DataField="ContactHp" HeaderText="Contact HP" />
                  <asp:BoundField DataField="ISOCertNo" HeaderText="ISO Sertificate No" SortExpression="ISOCertNo"  />
                  <asp:BoundField DataField="SIUPNo" HeaderText="SIUP No" />
                  <asp:BoundField DataField="FgSubkon" HeaderText="Supplier Subcont" />
                  <asp:BoundField DataField="WrhsSubkon" SortExpression="WrhsSubkon" HeaderText="Warehouse Subcont Code"></asp:BoundField>
                  <asp:BoundField DataField="WrhsSubkonName" SortExpression="WrhsSubkonName" HeaderText="Warehouse Subcont"></asp:BoundField>
                  <asp:BoundField DataField="FgActive" HeaderText="Active" />
              </Columns>
          </asp:GridView>
          </div>
          
          <asp:Panel runat="server" ID ="pnlNav" Visible="false">
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" /> &nbsp &nbsp &nbsp</asp:Panel>
    </asp:Panel>    
    
    <asp:Panel runat="server" ID="pnlView" Visible="false">
      <table>
        <tr>
            <td class="style1">Supplier Code </td>
            <td class="style2">:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbCode" AutoPostBack = "true"  MaxLength="12" Width="149px"/> &nbsp; &nbsp; </td>            
            <td class="style4">&nbsp;</td>
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">Supplier Name </td>
            <td class="style2">:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbName" MaxLength="60" Width="200px"/> &nbsp; &nbsp; </td>            
            <td class="style4">&nbsp;</td>
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>      
        <tr>
            <td class="style1">Member Of Group</td>
            <td class="style2">:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbMemberOfGroup" MaxLength="100" Width="200px"/> &nbsp; &nbsp; </td>            
            <td class="style4">&nbsp;</td>
            <td class="style1">Sister Company</td>
            <td class="style2">:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbSisterCompany" MaxLength="100" Width="200px"/>  </td>            
            <td class="style4">&nbsp;</td>
            
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
            
            
        </tr>
        
        <tr>
        
        </tr>
               
        <tr>
            <td class="style1">Supplier Class </td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlSuppClass" runat="server" CssClass="DropDownList" Width="205px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style1">Supplier Type </td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlSuppType" runat="server" CssClass="DropDownList" Width="205px"></asp:DropDownList></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
      
          <tr>
            <td class="style1">Address 1</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbAddress1" runat="server" MaxLength="100" TextMode="MultiLine" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style1">Address 2</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbAddress2" runat="server" MaxLength="100" TextMode="MultiLine" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
       
        <tr>
            <td class="style1">City</td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlCity" runat="server" CssClass="DropDownList" Width="205px"></asp:DropDownList></td>
            <td class="style4">&nbsp;&nbsp;</td>
              
            <td class="style6">Postal Code</td>
            <td class="style5">:</td>
            <td class="style3"><asp:TextBox ID="tbPostCode" runat="server" MaxLength="10" CssClass="TextBox" Width="200px" /></td>
        </tr>
        <tr>
            <td class="style1">Telephone</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbPhone" runat="server" MaxLength="40" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
              
            <td class="style6">Fax</td>
            <td class="style5">:</td>
            <td class="style3"><asp:TextBox ID="tbFax" runat="server" MaxLength="40" CssClass="TextBox" Width="200px" /></td>
        </tr>
        <tr> 
            <td class="style1">Email</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbEmail" runat="server" MaxLength="50" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style1">Currency</td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlCurr" runat="server" CssClass="DropDownList"></asp:DropDownList></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
       
        <tr>
            <td class="style1">Payment Term</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbTerm" runat="server" MaxLength="10" CssClass="TextBox" Width="41px" AutoPostBack="True" />
            &nbsp;<asp:DropDownList ID="ddlTerm" runat="server" CssClass="DropDownList"  Width="152px" AutoPostBack="True">
            </asp:DropDownList>
            </td>
            <td class="style4">&nbsp;</td>
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">PkKP</td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlPKP" runat="server" AutoPostBack="True" CssClass="DropDownList" Width="44px">
                <asp:ListItem>Y</asp:ListItem>
                <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style4">&nbsp;</td>
            <td class="style6">NPWP</td>
            <td class="style5">:</td>
            <td class="style3"><asp:TextBox ID="tbNPWP" runat="server" MaxLength="25" CssClass="TextBox" Width="200px" /></td>
        </tr>
        <tr>
            <td class="style1">PPN</td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlPPN" runat="server" AutoPostBack="True" CssClass="DropDownList"  Width="44px">
                <asp:ListItem>Y</asp:ListItem>
                <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style4">&nbsp;</td>
            <td class="style6">&nbsp;</td>
            <td class="style5">&nbsp;</td>
            <td class="style3">&nbsp;</td>
        </tr>
        <tr>
            <td class="style7">Director Name</td>
            <td class="style8">:</td>
            <td class="style9"><asp:TextBox ID="tbDirectorName" runat="server" MaxLength="50" CssClass="TextBox" Width="200px" /></td>
            <td class="style10"></td>
            
            
            <td class="style7">No KTP</td>
            <td class="style8">:</td>
            <td class="style9"><asp:TextBox ID="tbNoKTPHd" runat="server" MaxLength="20" CssClass="TextBox" Width="200px" /></td>
            <td class="style10"></td>
              
            <td class="style11">&nbsp;</td>
            <td class="style12">&nbsp;</td>
            <td class="style13">&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">Contact Person</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbContactPerson" runat="server" MaxLength="50" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
              
            <td class="style6">Contact Phone</td>
            <td class="style5">:</td>
            <td class="style3"><asp:TextBox ID="tbContactPh" runat="server" MaxLength="30" 
                    CssClass="TextBox" Width="200px" /></td>
        </tr>
        <tr>
            <td class="style1">Contact Title</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbContactTitle" runat="server" MaxLength="50" CssClass="TextBox" Width="200px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">Active</td>
            <td class="style5">:</td>
            <td class="style3"><asp:DropDownList ID="ddlAktive" runat="server" AutoPostBack="True" CssClass="DropDownList" Width="44px">
            <asp:ListItem>Y</asp:ListItem>
            <asp:ListItem>N</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="style1">ISO Certificate No</td>
            <td class="style2">:</td>
            <td><asp:TextBox ID="tbISOCertNo" runat="server" MaxLength="50" CssClass="TextBox" Width="198px" /></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">SIUP No</td>
            <td class="style5">:</td>
            <td class="style3"><asp:TextBox ID="tbSIUPNo" runat="server" MaxLength="50" CssClass="TextBox" Width="200px" /></td>
        </tr>
        <tr>
            <td class="style1">Supplier Subcont</td>
            <td class="style2">:</td>
            <td><asp:DropDownList ID="ddlFgSubkon" runat="server" AutoPostBack="True" CssClass="DropDownList" Width="44px">
            <asp:ListItem>Y</asp:ListItem>
            <asp:ListItem>N</asp:ListItem>
            </asp:DropDownList></td>
            <td class="style4">&nbsp;</td>
            
            <td class="style6">Warehouse Subcont</td>
            <td class="style5">:</td>
            <td class="style3"><asp:DropDownList ID="ddlWrhsSubkon" runat="server" 
                    CssClass="DropDownList" Width="205px"></asp:DropDownList></td>
        </tr>
        <tr>
           <td class="style1">&nbsp;</td>
           <td class="style2">&nbsp;</td>
           <td>
           <asp:Button ID="btnSaveHd" runat="server" class="bitbtndt btnsave" Text="Save" />									
           <asp:Button ID="btnCancelHd" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									                                    
           <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
           </td>
           <td class="style4">&nbsp;</td>
           <td class="style6">&nbsp;</td>
           <td class="style5">&nbsp;</td>
           <td class="style3">&nbsp;</td>
          </tr>
      </table>  
      
      <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Back" />
      <br />
      <br />
      
      <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
      Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
      StaticMenuItemStyle-CssClass="MenuItem" 
      StaticSelectedStyle-CssClass="MenuSelect">
      <StaticSelectedStyle CssClass="MenuSelect" />
      <StaticMenuItemStyle CssClass="MenuItem" />
      <Items>
      <asp:MenuItem Text="Detail Contact" Value="0"></asp:MenuItem>
      <asp:MenuItem Text="Detail Address" Value="1"></asp:MenuItem>
      <asp:MenuItem Text="Detail Bank" Value="2"></asp:MenuItem>
      <asp:MenuItem Text="Detail Product" Value="3"></asp:MenuItem>
      </Items>
    </asp:Menu>
      
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
      <asp:View ID="Tab1" runat="server">
       <br />      
         <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add"/>
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="false">
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
                            <%--<FooterTemplate>
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>                             
                            </FooterTemplate>--%>
                        </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="ContactName" HeaderText="Contact Name" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="ContactTitle" HeaderText="Contact Title" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Address1" HeaderText="Address 1"  HeaderStyle-Width="200" />
                            <asp:BoundField DataField="Address2" HeaderText="Address 2" HeaderStyle-Width="200"/>
                            <asp:BoundField DataField="Country" HeaderText="Country" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" HeaderStyle-Width="55"/>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-Width="100"/>
                           
                            <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="NoKTP" HeaderText="No KTP" SortExpression="NoKTP" HeaderStyle-Width="100"/>
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>     
        
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>Item No</td>
                    <td>:</td>
                    <td><asp:Label runat="server" ID="lbItemNo" /> </td>
                </tr>
                <tr>
                    <td>Contact Name</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbContactName" MaxLength="50" CssClass="TextBox" Width="225px" />
                    </td>                    
                </tr>
                <tr>
                    <td>Contact Title</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTitle" MaxLength="50" Width="225px"/></td>
                </tr>  
                 <tr>
                    <td>No KTP</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbNoKTP" runat="server" CssClass="TextBox" MaxLength="20" Width="225px" />
                    </td>
                </tr>              
                <tr>
                    <td>Address 1</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbContactAddr1" MaxLength="100" Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td>Address 2</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbContactAddr2" runat="server" CssClass="TextBox" MaxLength="100" Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td>Country</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlCountry" runat="server" CssClass="DropDownList" Width="230px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Postal Code</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" MaxLength="10" ID="tbPostalCode"/>
                    </td>
                </tr>
                <tr>
                    <td>Telephone</td>
                    <td>:</td>                    
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTelephone" MaxLength="40" Width="225px"/>
                    </td>
                </tr>
                <tr>
                    <td>Fax</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbContactFax" CssClass="TextBox" MaxLength="40" Width="225px" />                        
                    </td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbContactEmail" runat="server" CssClass="TextBox" MaxLength="50" Width="225px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save"/>
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/>
      </asp:Panel> 
      </asp:View>
    
      <asp:View ID="Tab2" runat="server">
      <br />
       <asp:Panel runat="server" ID="PnlAddress">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add"/>            
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="GridViewAddr" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="false">
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
                            <%--<FooterTemplate>
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>																                                
                            </FooterTemplate>--%>
                        </asp:TemplateField>
                            <asp:BoundField DataField="DeliveryCode" HeaderText="Delivery Code" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="DeliveryPlace" HeaderText="Delivery Place" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Address1" HeaderText="Address 1" HeaderStyle-Width="200"/>
                            <asp:BoundField DataField="Address2" HeaderText="Address 2" HeaderStyle-Width="200"/>
                            <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" HeaderStyle-Width="55"/>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Fax" HeaderText="Fax" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="ContactTitle" HeaderText="Contact Title" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="ContactHP" HeaderText="Contact Hp" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="150"/>
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel> 
            
       <asp:Panel runat="server" ID="PnlEditAddress" Visible="false">
            <table>
                <tr>
                    <td>Delivery Code</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDeliveryCode" MaxLength="12" Width="198px" /></td>
                </tr>
                <tr>
                    <td>Delivery Place</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDeliveryPlace" MaxLength="60" Width="198px" /></td>
                </tr>
                <tr>
                    <td>Address 1</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAddress1Addr" MaxLength="100" Width="198px" />
                    </td>
                </tr>
                <tr>
                    <td>Address 2</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbAddress2Addr" runat="server" CssClass="TextBox" MaxLength="100" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>Zip Code</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbZipCodeAddr" runat="server" CssClass="TextBox" MaxLength="10"/>
                    </td>
                </tr>
                <tr>
                    <td>Phone</td>
                    <td>:</td>
                    <td><asp:TextBox ID="TbPhoneAddr" runat="server" CssClass="TextBox" Width="136px" MaxLength="40"/>
                    </td>
                </tr>
                <tr>
                    <td>Fax</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbFaxAddr" runat="server" CssClass="TextBox" Width="138px" MaxLength="40"/>
                    </td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbEmailAddr" runat="server" CssClass="TextBox" Width="267px" MaxLength="50"/>
                    </td>
                </tr>
                <tr>
                    <td>Contact Person</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbPersonAddr" runat="server" CssClass="TextBox" MaxLength="50" Width="271px" />
                    </td>
                </tr>
                <tr>
                    <td>Contact Title</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbTitleAddr" runat="server" CssClass="TextBox" MaxLength="50" Width="269px" />
                    </td>
                </tr>
                <tr>
                    <td>Contact Hp</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbHpAddr" runat="server" CssClass="TextBox" MaxLength="30" Width="269px" />
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkAddr" runat="server" CssClass="TextBox" MaxLength="255" Width="269px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveAddr" runat="server" class="bitbtndt btnsave" Text="Save"/>
            <asp:Button ID="btnCancelAddr" runat="server" class="bitbtndt btncancel" Text="Cancel"/>            
       </asp:Panel>    
      </asp:View>
      
      <asp:View ID="Tab3" runat="server">
       <br />
        <asp:Panel runat="server" ID="PnlBank">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddBank" Text="Add"/>
            <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="false">
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
                            <%--<FooterTemplate>
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>																	
                            </FooterTemplate>--%>
                        </asp:TemplateField>
                            <asp:BoundField DataField="Item" HeaderText="No" />
                            <asp:BoundField DataField="Bank" HeaderText="Bank" HeaderStyle-Width="55" />
                            <asp:BoundField DataField="BankName" HeaderText="Bank Name" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="AccountNo" HeaderText="Account No" HeaderStyle-Width="100"/>
                            <asp:BoundField DataField="AccountName" HeaderText="Account Name" HeaderStyle-Width="200"/>
                            <asp:BoundField DataField="SwiftCode" HeaderText="Swift Code" HeaderStyle-Width="150"/>
                            <asp:BoundField DataField="Branch" HeaderText="Branch" HeaderStyle-Width="150"/>
                    </Columns>
                </asp:GridView>
          </div>   
          <br />
       </asp:Panel>      
       
       <asp:Panel runat="server" ID="PnlEditBank" Visible="false">
            <table>
                <tr>
                    <td>Item No</td>
                    <td>:</td>
                    <td><asp:Label runat="server" ID="lbItemBank" /> </td>
                </tr>
                <tr>
                    <td>Bank</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlBank" runat="server" CssClass="DropDownList" Height="16px" Width="198px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Account No</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAccounNo" runat="server" CssClass="TextBox" MaxLength="30" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>Account Name</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAccountName" runat="server" CssClass="TextBox" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td>Swift Code</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbSwiftCode" runat="server" CssClass="TextBox" Width="136px" MaxLength="30"/>
                    </td>
                </tr>
                <tr>
                    <td>Branch</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbBranch" runat="server" CssClass="TextBox" Width="138px" MaxLength="100"/>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save"/>									
            <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel"/>									
       </asp:Panel> 
      </asp:View>
      
      <asp:View ID="Tab4" runat="server">
      </asp:View>
      
   </asp:MultiView>    
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
