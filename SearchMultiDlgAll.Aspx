<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchMultiDlgAll.Aspx.vb" Inherits="SearchMultiDlgAll" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select</title>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <asp:Panel runat="server" ID="pnlHd" Visible="false">
     <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                                    
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="v"/>                  
             </td>
        </tr>
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr style="width:100%">
          <td style="width:100px;text-align:right">
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>          
          <td>           
              <asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
              <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
              </asp:DropDownList>
          </td>              
        <%--</tr>                
        <tr>--%>
          <td style="width:68px;text-align:right">
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlNotasi3" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>          
          <td>           
              <asp:TextBox runat="server" ID ="tbfilter3" CssClass="TextBox"/> 
              <asp:DropDownList runat="server" ID="ddlField3" CssClass="DropDownList" >
              </asp:DropDownList>
          </td>              
        </tr>
        <tr style="width:100%">
          <td style="width:100px;text-align:right">
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlNotasi4" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>          
          <td>           
              <asp:TextBox runat="server" ID ="tbfilter4" CssClass="TextBox"/> 
              <asp:DropDownList runat="server" ID="ddlField4" CssClass="DropDownList" >
              </asp:DropDownList>
          </td>              
        <%--</tr>                
        <tr>--%>
          <td style="width:68px;text-align:right">
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlNotasi5" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>          
          <td>           
              <asp:TextBox runat="server" ID ="tbfilter5" CssClass="TextBox"/> 
              <asp:DropDownList runat="server" ID="ddlField5" CssClass="DropDownList" >
              </asp:DropDownList>
          </td>              
        </tr>                
      </table>      
      </asp:Panel>
      <table> 
      <tr>
        <td style="width:100px;text-align:right"  >Order By :</td>            
        <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlOrderBy" >
            </asp:DropDownList>      
        </td>            
        </tr>        
      </table>
      </asp:Panel>
    <table width="80%"> 
    <tr>
    <td>
    <asp:Button class="bitbtn btnapply" runat="server" ID="btnApply" Text="Apply" />     
    </td>
   <td width="70%" align="right">
        <asp:Label ID="lbpagesize" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td width="30%" align="left">
        <asp:DropDownList runat="server" CssClass="DropDownList" 
           ID="ddlPager" AutoPostBack="true">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
              </asp:DropDownList>
    </td>
    
   <%-- <td>
    <asp:Button ID="btnUnSelect" runat="server" Text="UnSelect All" AutoPostBack="true">                    
                </asp:Button>        
     </td>--%>
     </tr>
     </table> 
    <div style="border:0px  solid; width:100%; height:380%; overflow:auto;">
    <dx:ASPxGridView ID="GridView1" runat="server" EmptyDataText="There are no data record(s) to display."
    AllowPaging="True">    
  <%--<Columns>
    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
    <HeaderTemplate><input type="checkbox" onclick="GridView1.SelectAllRowsOnPage(this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate>
    <HeaderStyle HorizontalAlign="Center" />
    </dx:GridViewCommandColumn>                                                    
  </Columns>--%>
  <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
       
  </dx:ASPxGridView>
  
  <%--<asp:GridView ID="GridView1" runat="server" CssClass="Grid" AllowPaging="True"  >
	<HeaderStyle CssClass="GridHeader"></HeaderStyle>
	<RowStyle CssClass="GridItem" Wrap="false" />
	<AlternatingRowStyle CssClass="GridAltItem"/>
	<PagerStyle CssClass="GridPager"  />
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                oncheckedchanged="cbSelectHd_CheckedChanged" />
            </HeaderTemplate>
            <AlternatingItemTemplate>
                <asp:CheckBox ID="cbSelect" runat="server" />
            </AlternatingItemTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="cbSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView> --%>
    </div> 
     <table width="80%"> 
    <tr>
    <td>
    <asp:Button class="bitbtn btnapply" runat="server" ID="btnApply2" Text="Apply" />     
    </td>
   <td width="70%" align="right">
        <asp:Label ID="Label1" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td width="30%" align="left">
        <asp:DropDownList runat="server" CssClass="DropDownList" 
           ID="ddlPager2" AutoPostBack="true">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
              </asp:DropDownList>
    </td>
    </tr>
     </table>     
    </div>
    
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red" Font-Bold="true" Font-Size="Medium" ></asp:Label>
    </form>
</body>
</html>
