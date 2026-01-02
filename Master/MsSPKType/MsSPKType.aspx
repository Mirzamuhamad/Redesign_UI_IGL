<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSPKType.aspx.vb" Inherits="Tugas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Template SPK File</title>
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
     <div class="H1">Template SPK File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:150px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Type SPK" Value="TypeSPKCode"></asp:ListItem>
                    <asp:ListItem Text="Type SPK File" Value="TypeSPKName"></asp:ListItem>
               
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
            <td style="width:150px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Type SPK" Value="TypeSPKCode"></asp:ListItem>
                    <asp:ListItem Text="Type SPK File" Value="TypeSPKName"></asp:ListItem>               
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
				            <asp:BoundField DataField="TypeSPKCode" HeaderText="Code" HeaderStyle-Width="140" SortExpression="TugasCode"/>
							<asp:BoundField DataField="TypeSPKName" HeaderText="Name" HeaderStyle-Width="300" SortExpression="TugasName"/>                            
                            <asp:BoundField DataField="Pasal_1" HeaderText="Pasal 1" SortExpression="Pasal_1"/>
                            <asp:BoundField DataField="Point_1" HeaderText="Point Pasal Satu" SortExpression="Point_1"/>
                            
                            <asp:BoundField DataField="Pasal_2" HeaderText="Pasal 2" SortExpression="Pasal_2"/>
                            <asp:BoundField DataField="Point_2" HeaderText="Title" SortExpression="Point_2"/>
                            
                            <asp:BoundField DataField="Pasal_3" HeaderText="Pasal 3" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_3" HeaderText="Point Pasal Dua" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_4" HeaderText="Pasal 4" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_4" HeaderText="Point Pasal Tiga" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_5" HeaderText="Pasal 5" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_5" HeaderText="Point Pasal Empat" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_6" HeaderText="Pasal 6" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_6" HeaderText="Point Pasal Enam" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_7" HeaderText="Pasal 7" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_7" HeaderText="Point Pasal Tujuh" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_8" HeaderText="Pasal 8" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_8" HeaderText="Point Pasal Delapan" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_9" HeaderText="Pasal 9" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_9" HeaderText="Point Pasal Sembilan" SortExpression="Title"/>
                            
                            <asp:BoundField DataField="Pasal_10" HeaderText="Pasal 10" SortExpression="Title"/>
                            <asp:BoundField DataField="Point_10" HeaderText="Point Pasal Sepuluh" SortExpression="Title"/> 		
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      

      
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
                   <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
                    <StaticSelectedStyle CssClass="MenuSelect" />
                    <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Pasal 1 s/d 5" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Pasal 6 s/d 10" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        </br>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
              <asp:View ID="Tab1" runat="server">
                  <table>
                        <tr>
                            <td>Type SPK Code</td>
                            <td>:</td> 
                            <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
                        </tr>
                        <tr>
                            <td>Type SPK Name</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/></td>
                        </tr>

                        <tr>
                            <td>Pasal 1</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"   CssClass="TextBox" ID="tbPasal1" ValidationGroup="Input" Width="300px"/></td>
                        </tr>
                        <tr>
                            <td>Poin Pasal 1 </td>
                            <td>:</td>
                            <td>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbNumber1" ValidationGroup="Input" Width="20px" Height="150px"/>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbpoint1" ValidationGroup="Input" Width="510px" Height="150px"/>
                            </td>
                            
                            <td>                            
                            Untuk memuncukan paket pekerjaan ketikan : <b>[Jenis]</b>
                            </br>
                            </br>
                            Untuk memuncukan nama penerima tugas ketikan : <b>[Nama]</b>
                            </br>
                            </br>
                            Untuk memuncukan alamat Pekerjaan ketikan   : <b>[Alamat]</b>
                            </td>
                            
                        </tr>
                        
                        
                         <tr>
                            <td>Pasal 2</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal2" ValidationGroup="Input" Width="300px"/></td>
                            
                        </tr>
                        <tr>
                            <td>Poin Pasal 2</td>
                            <td>:</td>
                            <td>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber2" ValidationGroup="Input" Width="20px" Height="150px"/>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbPoint2" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                            <td>                            
                            Untuk memuncukan durasi pekerjaan ketikan : <b>[durasi]</b>
                            </br>
                            </br>
                            Untuk memuncukan tanggal mulai pekerjaan ketikan : <b>[mulai]</b>
                            </br>
                            </br>
                            Untuk memuncukan tanggal selesai Pekerjaan ketikan   : <b>[selesai]</b>
                            </td>
                            
                        </tr>
                        
                         <tr>
                            <td>Pasal 3</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal3" ValidationGroup="Input" Width="300px"/></td>
                            
                        </tr>
                        <tr>
                            <td>Poin Pasal 3</td>
                            <td>:</td>
                            <td>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber3" ValidationGroup="Input" Width="20px" Height="150px"/>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbPoint3" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                            
                        </tr>
                        
                        <tr>
                            <td>Remark Rincian Biaya</td>
                            <td>:</td>
                            <td>
                            
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbRemarkRincian" ValidationGroup="Input" Width="540px" Height="50px"/></td>
                            
                        </tr>
                        
                         <tr>
                            <td>Pasal 4</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal4" ValidationGroup="Input" Width="300px"/></td>
                            
                        </tr>
                        <tr>
                            <td>Poin Pasal 4</td>
                            <td>:</td>
                            <td>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber4" ValidationGroup="Input" Width="20px" Height="150px"/>
                            <asp:TextBox runat="server" TextMode ="MultiLine"  spellcheck="false" CssClass="TextBox" ID="tbPoint4" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                            
                        </tr>
                        
                         <tr>
                            <td>Pasal 5</td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal5" ValidationGroup="Input" Width="300px"/></td>
                            
                        </tr>
                        <tr>
                            <td>Poin Pasal 5</td>
                            <td>:</td>
                            <td>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber5" ValidationGroup="Input" Width="20px" Height="150px"/>
                            <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbPoint5" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                            
                           
                        </tr>
                    </table>
              </asp:view>
              
              <asp:View ID="Tab2" runat ="server" >
              	<table>
                    <tr>
                        <td>Pasal 6</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal6" ValidationGroup="Input" Width="300px"/></td>
                    </tr>
                    <tr>
                        <td>Poin Pasal 6 </td>
                        <td>:</td>
                        <td>
                        <asp:TextBox runat="server" TextMode ="MultiLine"  spellcheck="false" CssClass="TextBox" ID="tbNumber6" ValidationGroup="Input" Width="20px" Height="150px"/>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbpoint6" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                    </tr>
                    
                    
                     <tr>
                        <td>Pasal 7</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal7" ValidationGroup="Input" Width="300px"/></td>
                    </tr>
                    <tr>
                        <td>Poin Pasal 7</td>
                        <td>:</td>
                        <td>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbNumber7" ValidationGroup="Input" Width="20px" Height="150px"/>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPoint7" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                    </tr>
                    
                     <tr>
                        <td>Pasal 8</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal8" ValidationGroup="Input" Width="300px"/></td>
                    </tr>
                    <tr>
                        <td>Poin Pasal 8</td>
                        <td>:</td>
                        <td>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber8" ValidationGroup="Input" Width="20px" Height="150px"/>
                        <asp:TextBox runat="server" TextMode ="MultiLine"  spellcheck="false" CssClass="TextBox" ID="tbPoint8" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                    </tr>
                    
                     <tr>
                         <td>Pasal 9</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal9" ValidationGroup="Input" Width="300px"/></td>
                    </tr>
                    <tr>
                         <td>Poin Pasal 9</td>
                        <td>:</td>
                        <td>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber9" ValidationGroup="Input" Width="20px" Height="150px"/>
                        <asp:TextBox runat="server" TextMode ="MultiLine"  spellcheck="false" CssClass="TextBox" ID="tbPoint9" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                    </tr>
                    
                     <tr>
                        <td>Pasal 10</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPasal10" ValidationGroup="Input" Width="300px"/></td>
                    </tr>
                    <tr>
                        <td>Poin Pasal 10</td>
                        <td>:</td>
                        <td>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false"  CssClass="TextBox" ID="tbNumber10" ValidationGroup="Input" Width="20px" Height="150px"/>
                        <asp:TextBox runat="server" TextMode ="MultiLine" spellcheck="false" CssClass="TextBox" ID="tbPoint10" ValidationGroup="Input" Width="510px" Height="150px"/></td>
                    </tr>

                </table>
              </asp:View>
        </asp:MultiView>                
        
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
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    
    </form>
</body>
</html>
