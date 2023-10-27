<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchMultiDlg.Aspx.vb" Inherits="SearchMultiDlg2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select</title>
    <script type="text/javascript">
        function SelectCheck(id)
        {
            //get reference of GridView control
            var grid = document.getElementById("<%= GridView1.ClientID %>");
            //variable to contain the cell of the grid
            var cell;
            
            if (grid.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<grid.rows.length; i++)
                {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];
                    
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {           
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type =="checkbox")
                        {
                        //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <asp:Panel runat="server" ID="pnlHd" Visible="True">
     <table>
        <tr>
            <%--<td style="width:100px;text-align:right">Quick Search :</td>  --%>          
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                                    
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="v"/>                  
             </td>
             
             <td style="width:100px;text-align:right">show Record :</td>
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
        <%--<asp:Label ID="lbRecord" text = "records" runat="server"></asp:Label>--%>
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
       <%-- <td style="width:100px;text-align:right"  >Order By :</td> --%>           
        <td><asp:DropDownList CssClass="DropDownList" Visible = "False" runat="server" ID="ddlOrderBy" >
            </asp:DropDownList>      
        </td>            
        </tr>        
      </table>
      </asp:Panel>
<asp:Button class="bitbtn btnapply" runat="server" ID="btnApply" Text="Apply" />  
<br />&nbsp;
 
<div style="border:0px  solid; width:100%; height:380%; overflow:auto;">
<asp:GridView ID="GridView1" runat="server" CssClass="Grid" AllowPaging="True" >
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager"
						  />
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                oncheckedchanged="cbSelectHd_CheckedChanged" />
            </HeaderTemplate>
            <AlternatingItemTemplate>
                <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack ="true" oncheckedchanged="cbSelectItem_CheckedChanged" />
            </AlternatingItemTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack ="true" oncheckedchanged="cbSelectItem_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView> 
<br />&nbsp;
<asp:Button class="bitbtn btnapply" runat="server" ID="btnApply2" Text="Apply" />     
</div>      
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red" Font-Bold="true" Font-Size="Medium" ></asp:Label>
    </form>
</body>
</html>
