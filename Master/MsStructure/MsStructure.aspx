<%@ Page Language="VB" AutoEventWireup="False" CodeFile="MsStructure.aspx.vb" Inherits="MsStructure_MsStructureNew" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Site Plan</title>

   <style type="text/css">
       .modalPopup
      {
      	position:relative;
        top:20%;
        left:20%; 
        min-height: 75px;
        position: fixed;
        z-index: 2000;
        padding: 0;
        background-color: #000;             
      /*  border-radius: 6px;
        background-clip: padding-box;
        box-shadow: 0 5px 10px rgba(0, 0, 0, 0);*/
        border: 1px solid rgba(0, 0, 0, 0.2);
        min-width: 290px;
      }
       .modalBackground
      {
        position: fixed;
        top: 0;
        left: 0;
        background-color: #fff; /*000 */
        z-index: 2000;
       /*min-height: 100%;
        filter: alpha(opacity=50);        
        display: inline-block;
        opacity: 0.5;
        */
        width: 100%;
        overflow: hidden;
        height: 100px ;  
      }                
   </style>     
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">

        function ProgressCircle() {
            setTimeout(function() {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function() {
            ProgressCircle();
        });
      
    
      function SetLabelText() 
      {
          //var dropdown = document.getElementById("ddlLevel");
          //tbStructureCode.value = dropdown.options[dropdown.selectedIndex].value;
          
          //tbStructureCode.value = dropdown.options[dropdown.selectedIndex].text;
          //document.getElementById("Label3").innerHTML = dropdown.options[dropdown.selectedIndex].text;        
      }
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Site Plan</div>
     <hr />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="StructureCode" Value="StructureCode"></asp:ListItem>
                    <asp:ListItem Text="StructureName" Value="StructureName"></asp:ListItem> 
                    <asp:ListItem Text="ParentID" Value="ParentID"></asp:ListItem>                   
                    <asp:ListItem Text="LevelName" Value="LevelName"></asp:ListItem>    
                    <asp:ListItem Text="Active Kavling" Value="FgActiveName"></asp:ListItem>                                                         
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
            <td style="width:100px;text-align:right">
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="StructureCode" Value="StructureCode"></asp:ListItem>
                    <asp:ListItem Text="StructureName" Value="StructureName"></asp:ListItem>                   
                    <asp:ListItem Text="ParentID" Value="ParentID"></asp:ListItem>                   
                    <asp:ListItem Text="LevelName" Value="LevelName"></asp:ListItem>
                    <asp:ListItem Text="Active Kavling" Value="FgActiveName"></asp:ListItem>                     
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:FileUpload style="color: black;" ID="fuLocation" runat="server"  /><%--accept="application/xls" --%>
        <asp:DropDownList ID="ddlSheets" runat="server" Width="90px" AutoPostBack="true" CssClass="DropDownList"
                AppendDataBoundItems = "true">
        </asp:DropDownList>
      <asp:Button ID="btnUpload" runat="server" class="bitbtn btnadd" Width="110px" Text="Get from XL" OnClick="btnUpload_Click" />
      <asp:Button ID="btnImportDB" runat="server" class="bitbtn btnadd" Width="110px" Text="Import to DB" />
      <br/>&nbsp;								
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid" >
						<HeaderStyle CssClass="GridHeader" Wrap="false"  ></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				            <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                                  <ItemTemplate>
                                      <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                          <asp:ListItem Selected="True" Text="View" />
                                          <asp:ListItem Text="Edit" />
                                          <asp:ListItem Text="Delete" />
                                      </asp:DropDownList>
                                      <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                                      
                                  </ItemTemplate>
                                  <HeaderStyle Width="110px" />
                            </asp:TemplateField>
				            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-Width="10" SortExpression="ID"/>
							<asp:BoundField DataField="ParentID" HeaderText="Parent ID" HeaderStyle-Width="20" SortExpression="ParentID"/>
							<asp:BoundField DataField="StructureCode" HeaderText="Structure Code" HeaderStyle-Width="20" SortExpression="StructureCode"/>
							<asp:BoundField DataField="StructureName" HeaderText="Structure Name" HeaderStyle-Width="20" SortExpression="StructureName"/>
							<asp:BoundField DataField="LevelName" HeaderText="Level Name" HeaderStyle-Width="20" SortExpression="LevelName"/>
							<%-- 
                            <asp:BoundField DataField="Account" HeaderText="Account" HeaderStyle-Width="20" SortExpression="Account"/>
							<asp:BoundField DataField="AccountName" HeaderText="Account Name" HeaderStyle-Width="20" SortExpression="AccountName"/> 
                            --%>
							<asp:BoundField DataField="LandArea" HeaderText="Unsold Area" HeaderStyle-Width="20" DataFormatString="{0:#,##0.00}" ItemStyle-horizontalAlign ="Right" SortExpression="LandArea"/>
                            <asp:BoundField DataField="OriginalArea" HeaderText="Original Area" HeaderStyle-Width="20" DataFormatString="{0:#,##0.00}" ItemStyle-horizontalAlign ="Right" SortExpression="OriginalArea"/>
							<asp:BoundField DataField="BuildingArea" HeaderText="Building Area" HeaderStyle-Width="20" DataFormatString="{0:#,##0.00}" ItemStyle-horizontalAlign ="Right" SortExpression="BuildingArea"/>
                            <asp:BoundField DataField="FgActiveName" HeaderText="Active Area" HeaderStyle-Width="20" SortExpression="FgActiveName" ItemStyle-horizontalAlign ="Center"/>
                            <asp:BoundField DataField="StartDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Registration Date" HeaderStyle-Width="20" SortExpression="StartDate"/>
                            <asp:BoundField DataField="UserId" HeaderText=" Last Update By" HeaderStyle-Width="20" SortExpression="UserId"/>
                            <asp:BoundField DataField="UserDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Last Update" HeaderStyle-Width="20" SortExpression="Userdate"/>
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>ID</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" ValidationGroup="Input" Width="100px" />&nbsp;
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Parent ID</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbParentID" ValidationGroup="Input" Width="100px" />&nbsp;
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Structure Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbStructureCode" ValidationGroup="Input" Width="150px" />&nbsp;
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    <%--<asp:Label ID="lbGetData" runat="server" ForeColor="Black" Text="" Visible="True" ></asp:Label>--%>
                </td>
            </tr> 
            <tr>
                <td>Structure Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbStructureName" ValidationGroup="Input" Width="150px"/>&nbsp;
                <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
             <tr>
                <td>Level</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLevel" ValidationGroup="Input" Width="155px"  
                     onchange="SetLabelText();"  AutoPostBack="False">
                    </asp:DropDownList>
                </td>
            </tr>
                       
            <%--<tr>
                <td>Area</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlArea" ValidationGroup="Input" MaxLength = "20" 
                        Width = "200px" AutoPostBack="True" onchange="setLabelText();" >
                    </asp:DropDownList>&nbsp;
                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
               </td>
            </tr> 
            <tr>          
                <td>Kawasan</td>
                <td>:</td>
                <td>
                  <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlKawasan" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" AutoPostBack="True" >
                  </asp:DropDownList>&nbsp;
                  <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Block</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlBlock" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" >
                    </asp:DropDownList>&nbsp;
                   <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>           
            <tr>
                <td>Cluster</td>
                <td>:</td>
                <td>
                  <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCluster" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" >
                  </asp:DropDownList>&nbsp;
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Kavling</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlKavling" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px">
                    </asp:DropDownList>&nbsp;
                   <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>   
            <tr>
                <td>Unit</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlUnit" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" >
                    </asp:DropDownList>&nbsp;
                   <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr> --%>
            <tr>
                <td>Sisa Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbLuasLahan" ValidationGroup="Input" Width="100px" /></td>
            </tr>

            <tr>
                <td>Original Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbOriginalArea" ValidationGroup="Input" Width="100px" /></td>
            </tr>
            
             <tr>
                <td>Building Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbLuasBangunan" ValidationGroup="Input" Width="100px"/></td>
            </tr>
            
            <tr>
                <td>Account</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAccount" ValidationGroup="Input" Width="100px"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccountName" Enabled="false" Width="225px"/>                
                <asp:Button Class="btngo" ID="btnAccount" Text="..." runat="server" ValidationGroup="Input" />   </td>
            </tr>
                    
                     
        <%--    <tr>
                <td>Est Start Tanam</td>
                <td>:</td>
                <td> <BDP:BasicDatePicker ID="tbEstStartPlant" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Start Tanam</td>
                <td>:</td>
                <td> <BDP:BasicDatePicker ID="tbStartPlant" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate"
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                </td>
            </tr>
             <tr>
                <td>SPH </td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbSPH" ValidationGroup="Input" Width="200px" Enabled="False" ReadOnly="True" /></td>
            </tr>    --%>
            

            <tr> 
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Reset" />
                    &nbsp;
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
                </td>
            </tr>
        </table>
      </asp:Panel>
      
      <asp:Panel runat="server" ID="pnlUpload" >
        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView id="GVExcelFile" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="true" AllowPaging="false" CssClass="Grid" PageSize="50" >
			<HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
			<RowStyle CssClass="GridItem" wrap="false" />
			<AlternatingRowStyle CssClass="GridAltItem"/>
			<FooterStyle CssClass="GridFooter" />
			<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
			<Columns> 
										
    		</Columns>   		
          </asp:GridView>
        </div> 
      </asp:Panel>
                    
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    
    <div class="loading" align="center">
      Loading. Please wait.<br />
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    
    </form>
</body>
</html>
