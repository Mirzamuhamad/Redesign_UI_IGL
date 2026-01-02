<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTugas.aspx.vb" Inherits="Tugas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notaris File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
       <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    
    <script type="text/javascript">
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
    </script>
      
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Pemberi Tugas File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Pemberi Tugas" Value="TugasName"></asp:ListItem>
                    <asp:ListItem Text="No KTP" Value="NoKTP"></asp:ListItem>
                    <asp:ListItem Text="Title" Value="Title"></asp:ListItem>
                    <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                    <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                    <asp:ListItem Text="City" Value="City"></asp:ListItem>               
                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                    <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" Visible="false" ID="btnPrint" Text="Print"/>
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Pemberi Tugas" Value="TugasName"></asp:ListItem>
                    <asp:ListItem Text="No KTP" Value="NoKTP"></asp:ListItem>
                    <asp:ListItem Text="Title" Value="Title"></asp:ListItem>
                    <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                    <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                    <asp:ListItem Text="City" Value="City"></asp:ListItem>               
                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                    <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
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
				            <asp:BoundField DataField="TugasCode" HeaderText="Code" HeaderStyle-Width="140" SortExpression="TugasCode"/>
							<asp:BoundField DataField="TugasName" HeaderText="Name" HeaderStyle-Width="300" SortExpression="TugasName"/>                            
                            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"/>
                            <asp:BoundField DataField="NoKTP" HeaderText="No KTP " SortExpression="NoKTP"/>
                            <asp:BoundField DataField="Address1" HeaderText="Address" SortExpression="Address1"/> 
                            <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/>                           
                            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City"/>    
                            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone"/>  
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"/>  
                            <asp:BoundField DataField="NPWP" HeaderText="NPWP" SortExpression="NPWP"/>  		
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Pemberi Tugas Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Pemberi Tugas Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/></td>
            </tr>


            <tr>
                <td>No KTP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbNoKTP" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Title</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbTitle" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Address</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="200" TextMode ="MultiLine"  CssClass="TextBox" ID="tbAddress" ValidationGroup="Input" Width="300px"/></td>
            </tr>

             <tr>
                <td>Address 2</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="200" CssClass="TextBox" TextMode ="MultiLine" ID="tbAddress2" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Kota</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCity" runat="server" CssClass="DropDownList" Width="230px"></asp:DropDownList></td>
            </tr>


            <tr>
                <td>Phone</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="15" CssClass="TextBox" ID="tbPhone" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Email</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbEmail" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>NPWP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="20" CssClass="TextBox" ID="tbNpwp" ValidationGroup="Input" Width="300px"/></td>
            </tr>



            
        </table>
                    <br>
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btndelete" 
                        CommandName="Cancel" Text="Reset" />
                    &nbsp;
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />                                                                                            
               
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
