<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FindDlg1.ascx.vb" Inherits="UserControl_FindDlg1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>   

<script type="text/javascript">
function GetRowValue2(val,val2,Obj1, Obj2)
{
    // hardcoded value used to minimize the code.
    // ControlID can instead be passed as query string to the popup window
    window.opener.document.getElementById(Obj1).value = val;    
    window.opener.document.getElementById(Obj2).value = val2;            
    window.close();    
}
function GetRowValue3(val,val2,val3,Obj1,Obj2,Obj3)
{
    window.opener.document.getElementById(Obj1).value = val;    
    window.opener.document.getElementById(Obj2).value = val2;    
    window.opener.document.getElementById(Obj3).value = val3;    
    window.close();    
}
function GetRowValue4(val,val2,val3,val4, Obj1,Obj2,Obj3,Obj4)
{
    window.opener.document.getElementById(Obj1).value = val;    
    window.opener.document.getElementById(Obj2).value = val2;    
    window.opener.document.getElementById(Obj3).value = val3;
    window.opener.document.getElementById(Obj4).value = val4;
    window.close();    
}
function GetRowValue5(val,val2,val3,val4,val5, Obj1,Obj2,Obj3,Obj4,Obj5 )
{
    window.opener.document.getElementById(Obj1).value = val;    
    window.opener.document.getElementById(Obj2).value = val2;    
    window.opener.document.getElementById(Obj3).value = val3;
    window.opener.document.getElementById(Obj4).value = val4;
    window.opener.document.getElementById(Obj5).value = val5;
    window.close();    
}

</script>    

<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
<style type=”text/css”>
.modalBackground
{
background-color: Gray;
filter: alpha(opacity=70);
opacity: 0.7;
}
.modalPopup
{
background-color: White;
height: 250px;
width:500px;
text-align:left;
}
</style>

<asp:Button ID="btnShowPopup" runat="server" Style="display: none" />

<cc1:ModalPopupExtender BackgroundCssClass="BackgroundStyle"
    runat="server" PopupControlID="divfind" ID="ModalPopupExtender1"
    TargetControlID="btnShowPopup" />

<asp:Panel id="divfind" runat="server" CssClass="modalBackground">
    <table>
        <tr>
            <td style="width:100px;text-align:right"  >Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    </asp:DropDownList>
                  <asp:Button runat="server" ID="btnSearch1" Text="Go" AutoPostBack="false"/>
                  <asp:ImageButton ID="btnSearch" runat="server" AutoPostBack="false"  
                    ImageUrl="../Image/btngoon.png"                    
                    ImageAlign="AbsBottom" />                
                  <%--<asp:Button runat="server" CssClass="Button" ID="btnExpand" Text="..." />                            --%>
                  <asp:ImageButton ID="btnExpand" runat="server"  
                    ImageUrl="../Image/btndownon.png"
                    ImageAlign="AbsBottom" />                
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
      <br />
     <table width="80%"> 
    <tr>
    <td>
    <asp:ImageButton runat="server" ID = "btnApply" 
    ImageUrl="../Image/btnApplyon.png"
    ImageAlign="AbsBottom" /> 
    </td>
   <td class="style1" width="70%" align="right">
        <asp:Label ID="lbPager" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td class="style1" align="left">
        <asp:DropDownList runat="server" CssClass="DropDownList" 
           ID="ddlPager" AutoPostBack="true">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
              </asp:DropDownList>
         <asp:Label ID="lbRecord" text = "records" runat="server"></asp:Label>
    </td>
    </tr>
     </table>     
      <dx:ASPxGridView ID="GridView1" runat="server" EmptyDataText="There are no data record(s) to display." AllowPaging="True" >
                          <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                          <SettingsBehavior AllowFocusedRow="True" />                          
                        </dx:ASPxGridView>
    <table width="80%"> 
    <tr>
    <td>
    <asp:ImageButton runat="server" ID = "btnApply2" 
    ImageUrl="../Image/btnApplyon.png"
    ImageAlign="AbsBottom" /> 
    </td>
   <td class="style1" width="70%" align="right">
        <asp:Label ID="lbPager2" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td class="style1" align="left">
        <asp:DropDownList runat="server" CssClass="DropDownList" 
           ID="ddlPager2" AutoPostBack="true">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
              </asp:DropDownList>        
        <asp:Label ID="lbRecord2" text = "records" runat="server"></asp:Label>
    </td>
    </tr>
     </table>  
    </asp:panel>      
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"></asp:Label>    