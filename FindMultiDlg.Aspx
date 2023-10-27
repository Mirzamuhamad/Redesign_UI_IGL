<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindMultiDlg.Aspx.vb" Inherits="FindMultiDlg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Search Data</title>
    
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
/*
function CheckAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                inputList[i].checked = true;
            }
            else {
                if (row.rowIndex % 2 == 0) {
                }
                else {
                }
                inputList[i].checked = false;
            }
        }
    }
}*/

function CheckAll(Checkbox) {
    var GridVwHeaderCheckbox = document.getElementById("<%=GridView1.ClientID %>");
    for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
        GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
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
                <!-- <td style="width:100px;text-align:right"  >Quick Search :</td>             -->
                <td>                                  
                      <asp:Button class="btngo" Visible="False" runat="server" ID="btnExpand" Text="v"/>                  
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
          <table >
                <tr>
                <!-- <td style="width:100px;text-align:right"  >Order By :</td>             -->
                <td><asp:DropDownList Visible="False" CssClass="DropDownList" runat="server" ID="ddlOrderBy" >
                    </asp:DropDownList>      
                </td>            
                </tr>        
          </table>      
        </asp:Panel>
      <%--<br />--%>
     <table> 
    <tr>

    <td>
    <asp:Button class="bitbtn btnapply" Visible="False" runat="server" ID="btnApply1" Text="Apply" />                                      
    </td>
   <td >
        <asp:Label Visible="False" ID="lbPager" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td>
        <asp:DropDownList Visible="False" runat="server" CssClass="DropDownList" 
           ID="ddlPager" AutoPostBack="true" height="20">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
        </asp:DropDownList>
         <asp:Label Visible="False" ID="lbRecord" text = "records" runat="server"></asp:Label>
    </td>
    </tr>
     </table>     

     <table>
        <tr>
            <td>
                <asp:TextBox CssClass="TextBox" placeholder="Search Data" runat="server" width="150px" ID ="tbFilter"/> 
                      <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                        </asp:DropDownList>                  
                      <asp:Button class="bitbtn btnsearch"  runat="server" ID="btnSearch" Text="Search" width="80px" />
            </td>
        </tr>
         <tr>
             <td>
                 <ClientSideEvents RowDblClick="function(s, e) {GridView1.PerformCallback(s.GetFocusedRowIndex()); window.opener.document.form1.submit(); window.close();}" />
                <ClientSideEvents RowDblClick="function(s, e) {GridView1.PerformCallback(e.visibleIndex); window.opener.document.form1.submit(); window.close(); }" />
                
              <asp:GridView ID="GridView1" runat="server" AllowPaging="False" CssClass="Grid" >                  
                  <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                  <RowStyle CssClass="GridItem" Wrap="false" />        
                  <AlternatingRowStyle CssClass="GridAltItem"/>
                  <PagerStyle CssClass="GridPager" />     
                <Columns>
                  <asp:TemplateField><%--HeaderText="Action" headerstyle-width="100px"--%>
                    <HeaderTemplate> 
                      <asp:CheckBox ID="chkSelectAll" runat="server" onclick = "CheckAll(this);" /><%--style="width: 50px;"> --%> 
                    </HeaderTemplate>            
                    <ItemTemplate>
                      <asp:CheckBox ID="chkRow" runat="server" class="chkSelect" onclick = "Check_Click(this)" />
                    </ItemTemplate>
                  </asp:TemplateField>
                </Columns>                         
              </asp:GridView>
             </td>
         </tr>
     </table>


    <table> 
    <tr>
    <td>
    <asp:Button ID="btnApply" runat="server" class="bitbtn btnapply" Text="Apply" /> <%--OnClick="GetSelectedRecords"--%>                                          
    </td>
   <td align="right">
        <asp:Label  Visible="False"  ID="lbPager2" text = "Page Size : " runat="server"></asp:Label>
   </td>
   <td  align="left">
        <asp:DropDownList Visible="False" runat="server"  CssClass="DropDownList" 
           ID="ddlPager2" AutoPostBack="true" height="20px">
                <asp:ListItem Selected="true" Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="25" Value="25"></asp:ListItem>            
                <asp:ListItem Text="50" Value="50"></asp:ListItem>            
                <asp:ListItem Text="100" Value="100"></asp:ListItem>            
                <asp:ListItem Text="500" Value="500"></asp:ListItem>            
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>            
              </asp:DropDownList>        
        <asp:Label Visible="False" ID="lbRecord2" text = "records" runat="server"></asp:Label>
    </td>
    </tr>
     </table>  
    </div>    
    </form>
    <asp:Label runat ="server" ID="lbStatus"></asp:Label>
</body>
</html>
