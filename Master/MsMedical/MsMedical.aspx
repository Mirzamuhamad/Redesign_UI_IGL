<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMedical.aspx.vb" Inherits="Master_MsMedical_MsMedical" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medical File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    
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
             
            var MaxTaken = document.getElementById("tbMaxTaken").value.replace(/\$|\,/g,""); 
            var CheckPlafondBy1Claim = document.getElementById("tbCheckPlafondBy1Claim").value.replace(/\$|\,/g,""); 
            var LeadTime = document.getElementById("tbLeadTime").value.replace(/\$|\,/g,"");          
            
            
//          
         try
         {                       
            document.getElementById("tbMaxTaken").value = setdigit(MaxTaken,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbCheckPlafondBy1Claim").value = setdigit(CheckPlafondBy1Claim,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbLeadTime").value = setdigit(LeadTime,'<%=ViewState("DigitHome")%>');            
           
        }catch (err){
            alert(err.description);
          }      
        }
        

        
        function postback()
        {
            __doPostBack('','');
        } 
    </script>
   <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
   <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Medical File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="true" Text="Medical Code" Value="Medical_Code"></asp:ListItem>
                      <asp:ListItem Text="Medical Name" Value="Medical_Name"></asp:ListItem>
                      <asp:ListItem Text="Medical Group" Value="Medical_Group_Code"></asp:ListItem>
                      <asp:ListItem Text="Medical Group Name" Value="Medical_Group_Name"></asp:ListItem>
                      <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>  
                      <asp:ListItem Text="Claim Family" Value="FgClaimFamily"></asp:ListItem>  
                      <asp:ListItem Text="Lead time" Value="Leadtime"></asp:ListItem>  
                      <asp:ListItem Text="Max Taken" Value="FgMaxTaken"></asp:ListItem> 
                      <asp:ListItem Text="Max Taken Time" Value="Max_Taken"></asp:ListItem> 
                      <asp:ListItem Text="Join Medical" Value="Join_Medical"></asp:ListItem> 
                      <asp:ListItem Text="Join Medical Name" Value="JoinMedicalName"></asp:ListItem>                                            
                      <asp:ListItem Text="Check Plafond" Value="FgCheckPlafond"></asp:ListItem> 
                      <asp:ListItem Text="Show Slip" Value="FgShowSlip"></asp:ListItem> 
                      <asp:ListItem Text="All Job Level" Value="FgAllJoblevel"></asp:ListItem>                       
                      <asp:ListItem Text="Check 1 Claim" Value="FgCheck1Claim"></asp:ListItem>                       
                      <asp:ListItem Text="Plafond 1 Claim" Value="Plafond1Claim"></asp:ListItem>                                             
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="Medical Code" Value="Medical_Code"></asp:ListItem>
                      <asp:ListItem Text="Medical Name" Value="Medical_Name"></asp:ListItem>
                      <asp:ListItem Text="Medical Group" Value="Medical_Group_Code"></asp:ListItem>
                      <asp:ListItem Text="Medical Group Name" Value="Medical_Group_Name"></asp:ListItem>
                      <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>  
                      <asp:ListItem Text="Claim Family" Value="FgClaimFamily"></asp:ListItem>  
                      <asp:ListItem Text="Lead time" Value="Leadtime"></asp:ListItem>  
                      <asp:ListItem Text="Max Taken" Value="FgMaxTaken"></asp:ListItem> 
                      <asp:ListItem Text="Max Taken Time" Value="Max_Taken"></asp:ListItem>                       
                      <asp:ListItem Text="Join Medical" Value="Join_Medical"></asp:ListItem> 
                      <asp:ListItem Text="Join Medical Name" Value="JoinMedicalName"></asp:ListItem>                                            
                      <asp:ListItem Text="Check Plafond" Value="FgCheckPlafond"></asp:ListItem> 
                      <asp:ListItem Text="Show Slip" Value="FgShowSlip"></asp:ListItem> 
                      <asp:ListItem Text="All Job Level" Value="FgAllJoblevel"></asp:ListItem>                       
                      <asp:ListItem Text="Check 1 Claim" Value="FgCheck1Claim"></asp:ListItem>                       
                      <asp:ListItem Text="Plafond 1 Claim" Value="Plafond1Claim"></asp:ListItem>                                             
                 </asp:DropDownList>               
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />	
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="False"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap = "false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="View"/>
                                <asp:ListItem Text="Edit" />
                                <asp:ListItem Text ="Delete"/>                                
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnGO" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>                               
                            </ItemTemplate>
                      <HeaderStyle Width="110px" />
                      </asp:TemplateField>
				            <asp:BoundField DataField="Medical_Code" HeaderText="Medical Code" HeaderStyle-Width="150px" SortExpression="Medical_Code"><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="Medical_Name" HeaderText="Medical Name" HeaderStyle-Width="200px" SortExpression="Medical_Name"><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="Medical_Group_Code" HeaderText="Medical Group" HeaderStyle-Width="150px" SortExpression="Medical_Group_Code"><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="Medical_Group_Name" HeaderText="Medical Group Name" HeaderStyle-Width="150px" SortExpression="Medical_Group_Name"><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-Width="200px" SortExpression="Gender"><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="FgClaimFamily" HeaderText="Claim Family" HeaderStyle-Width="30px" SortExpression="FgClaimFamily" ><HeaderStyle Width="30px" /></asp:BoundField>
							<asp:BoundField DataField="Leadtime" HeaderText="Lead Time" HeaderStyle-Width="80px" SortExpression="Leadtime"><HeaderStyle Width="80px" /></asp:BoundField>							
							<asp:BoundField DataField="FgMaxTaken" HeaderText="Max Taken" HeaderStyle-Width="150px" SortExpression="FgMaxTaken" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="Max_Taken" HeaderText="Max Taken Time" HeaderStyle-Width="200px" SortExpression="Max_Taken" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="Join_Medical" HeaderText="Join Medical" HeaderStyle-Width="200px" SortExpression="Join_Medical" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="JoinMedicalName" HeaderText="Join Medical Name" HeaderStyle-Width="150px" SortExpression="JoinMedicalName" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="FgCheckPlafond" HeaderText="Check Plafond" HeaderStyle-Width="150px" SortExpression="FgCheckPlafond" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="FgShowSlip" HeaderText="Show And Slip" HeaderStyle-Width="200px" SortExpression="FgShowSlip" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="FgAllJoblevel" HeaderText="All Job level" HeaderStyle-Width="150px" SortExpression="FgAllJoblevel" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="FgCheck1Claim" HeaderText="Check Plafond By 1 Claim" HeaderStyle-Width="150px" SortExpression="FgCheck1Claim" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="Plafond1Claim" HeaderText="Plafond By 1 Claim" HeaderStyle-Width="150px" SortExpression="Plafond1Claim" ><HeaderStyle Width="150px" /></asp:BoundField>							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
      </div>
    </asp:Panel>
     <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Medical Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMedicalCode" MaxLength="5" 
                        ValidationGroup="Input" /></td>
            </tr>
            <tr>
                <td>Medical Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="50" CssClass="TextBox" ID="tbMedicalName" 
                        Width="250px" ValidationGroup="Input"/></td>
            </tr>
            <tr>
                <td>Medical Group</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlMedicalGroup" Width="160px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList></td>
            </tr>   
            <tr>
                <td>Gender</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlGender" 
                        Width="100px">
                    <asp:ListItem >All</asp:ListItem>
                    <asp:ListItem >Male</asp:ListItem>                    
                    <asp:ListItem >Female</asp:ListItem>                                        
                    </asp:DropDownList>							    
                </td>
            </tr>         
            <tr>
                <td>Claim Family</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlClaim" 
                        Width="50px">
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>							    
                </td>
            </tr>       

            <tr>
                <td>Lead Time</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="4" CssClass="TextBox" ID="tbLeadTime" 
                        Width="50px" AutoPostBack="True" ValidationGroup="Input"/>
                     Days     
                </td>
            </tr>
            <tr>
                <td>Max Taken</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlMaxTaken" AutoPostBack = "true"                  
                        Width="50px" OnSelectedIndexChanged = "ddlMaxTaken_SelectedIndexChanged" >
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>	
                    <asp:TextBox runat="server" MaxLength="4" CssClass="TextBox" ID="tbMaxTaken" 
                        Width="50px" AutoPostBack="True" ValidationGroup="Input"/>
                    x Time
                 </td>  
            </tr>
            <tr>
                <td>Join Medical</td>
                <td>:</td>
                <td>
                <asp:TextBox ID="tbJoinMedical" runat="server" AutoPostBack="true" MaxLength = "20" 
                     CssClass="TextBox" ValidationGroup = "Input" />
                <asp:TextBox ID="tbJoinMedicalName" runat="server" CssClass="TextBoxR" Enabled="false" 
                     EnableTheming="True" Width="200px" />
                <asp:Button Class="btngo" ID="btnJoinMedical" Text="..." runat="server" ValidationGroup="Input" />                                                       
                </td> 
            
            </tr>            
            <tr>
                <td>Check Plafond</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCheckPlafond" 
                        Width="50px">
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>							    
                </td>
            </tr>
            <tr>
                <td>Show and Slip</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlShowSlip" 
                        Width="50px">
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>							    
                </td>
            </tr>
            <tr>
                <td>All Job Level</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlJobLevel" 
                        Width="50px">
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>							    
                </td>
            </tr>
            <tr>
                <td>Check Plafond By 1 Claim</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCheckPlafondClaim" AutoPostBack = "true"                   
                        Width="50px" OnSelectedIndexChanged = "ddlCheckPlafondClaim_SelectedIndexChanged" >
                    <asp:ListItem >Y</asp:ListItem>
                    <asp:ListItem >N</asp:ListItem>                                        
                    </asp:DropDownList>							    
                    <asp:TextBox ID="tbCheckPlafondBy1Claim" maxlength = "9" runat="server" CssClass="TextBox" ValidationGroup = "Input"
                     Width="100px" />
                </td>
            </tr>


            <tr>
                <td colspan="3" align="center">
                <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" ValidationGroup="Input"/>&nbsp;									
                <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Cancel" ValidationGroup="Input"/>&nbsp;                     
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>&nbsp;									                                    
                </td>
            </tr>
        </table>
      </asp:Panel>         
        
     </div>   
  
    <asp:SqlDataSource ID="dsMedicalGroup" runat="server"       
      SelectCommand="EXEC S_GetMsMedicalGroup">
    </asp:SqlDataSource>


    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
