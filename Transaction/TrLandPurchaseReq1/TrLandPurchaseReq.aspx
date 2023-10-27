<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPlanLand.aspx.vb" Inherits="TrLandPurchaseReq" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
     function OpenPopup() {
         window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }
     function openprintdlg() {
         var wOpens;
         wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
         wOpens.moveTo(0, 0);
         wOpens.resizeTo(screen.width, screen.height);
     }

     function OpenPopup() {
         window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }    
    </script>
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
            var _QtyOutput = parseFloat(document.getElementById("tbQtyM").value.replace(/\$|\,/g,""));
            var _QtyWO = parseFloat(document.getElementById("tbQtyT").value.replace(/\$|\,/g,""));
            var _QtyGood = parseFloat(document.getElementById("tbQtyB").value.replace(/\$|\,/g,""));
//            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
            var _QtyReject = parseFloat(document.getElementById("tbQtyS").value.replace(/\$|\,/g,""));
            
            
         
            document.getElementById("tbQtyM").value = setdigit(_QtyM,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyT").value = setdigit(_QtyT,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyB").value = setdigit(_QtyB,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyS").value = setdigit(_QtyS,'<%=VIEWSTATE("DigitQty")%>');            
            //alert("test 2");                                                
            
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
        <div class="H1">
             <asp:Label ID="lbltitle" runat="server" ForeColor="000000" Text="*"></asp:Label></div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, TransName, Division, DivisionName, StartDate, MasterPlanNo, QtyTarget, TargetType, Areal, QtyTarget, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                              <asp:ListItem>Status</asp:ListItem>
                              <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                              <asp:ListItem>Remark</asp:ListItem>
                            
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                        Show Records :
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        Rows
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                               <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                  <asp:ListItem>Status</asp:ListItem>
                                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                  <asp:ListItem>Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
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
                                    
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Prchase Req No">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" 
                            HeaderText="Prchase Req Date">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                      
                        <asp:BoundField DataField="Block" HeaderStyle-Width="200px" SortExpression="Block"
                            HeaderText="Block No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Kohir" HeaderStyle-Width="200px" SortExpression="Kohir"
                            HeaderText="Kohir No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Persil" HeaderStyle-Width="200px" SortExpression="Persil"
                            HeaderText="Persil No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Kohir" HeaderStyle-Width="200px" SortExpression="Kohir"
                            HeaderText="Kohir No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="AJBNo" HeaderStyle-Width="200px" SortExpression="AJBNo"
                            HeaderText="AJB No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SPHNo" HeaderStyle-Width="200px" SortExpression="SPHNo"
                            HeaderText="SPH No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SHMNo" HeaderStyle-Width="200px" SortExpression="SHMNo"
                            HeaderText="SHM No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        
                        <asp:BoundField DataField="LuasUkur" HeaderText="Luas Ukur" DataFormatString="{0:#,##0.##}" SortExpression="LuasUkur" />

                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                            HeaderText="Remark" SortExpression="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table>
                <tr>
                    <td>Prchase Req No</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="225px" Enabled="False"/>             
                    
                    </td>            
                </tr>
                <tr>
                    <td>Prchase Req Date</td>
                    <td>:</td>
                    <td>
                    <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                                ReadOnly = "true" ValidationGroup="Input"
                                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                DisplayType="TextBoxAndImage" 
                                TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>            
                </tr>    

                 <tr>
                    <td>No Girik Blok</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbBlok" CssClass="TextBox" Width="225px"/></td>

                    <td> </td>

                    <td>No AJB</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAJB" CssClass="TextBox" Width="225px"/></td>
                </tr>

                 <tr>
                    <td>No Girik Kohir</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbKohir" CssClass="TextBox" Width="225px"/></td>


                    <td> </td>

                    <td>No SPH</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSPH" CssClass="TextBox" Width="225px"/></td>
                </tr>      
                      

                <tr>
                    <td>No Girik Percil</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPercil" CssClass="TextBox" Width="225px"/></td>

                    <td> </td>

                    <td>No SHM</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSHM" CssClass="TextBox" Width="225px"/></td>
                </tr>   

                 <tr>
                    <td><asp:LinkButton ID="lbSeller" ValidationGroup="Input" runat="server" Text="Seller"/></td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" runat="server" ID="ddlseller" AutoPostBack="false" /></td>  
                    <td>  </td>

                     <td><asp:LinkButton ID="lbModerator" ValidationGroup="Input" runat="server" Text="Moderator"/></td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" runat="server" ID="ddlModerator" AutoPostBack="false" /></td>                  
                </tr>   
                
                 

                <tr>
                        <td>Luas</td>
                        <td>:</td>
                        <td colspan="7">
                            <table>
                                <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                    <td>SPPT</td>
                                    <td>AJB/SPH/SHM</td>
                                    <td>Luas Ukur</td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox ID="tbSPPT" runat="server" CssClass="TextBox" Width="100px"/></td>
                                    <td><asp:TextBox ID="tbAjbSphShm" ValidationGroup="Input" runat="server" CssClass="TextBox" width="100px"/></td>
                                    <td><asp:TextBox ID="tbLuasUkur" runat="server" CssClass="TextBox" Width="100px"/></td>                          
                                </tr>
                            </table>
                        </td>                
                </tr>

                <tr>
                    <td>Nilai Tanah/m<sup>2</sup></td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" AutoPostBack="true" ID="tbNilai" CssClass="TextBox" Width="225px"/></td>

                    <td> </td>

                    <td>Total</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotal" CssClass="TextBox" Width="225px"/></td>
                </tr> 

                <tr>
                        <td>Address</td>
                        <td>:</td>
                        <td colspan="7">
                            <table>
                                <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                    <td>Address</td>
                                    <td>Provinsi</td>
                                    <td>Kabupaten</td>                           
                                </tr>
                                <tr>
                                     <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAddress" CssClass="TextBox" Width="225px"/></td>  
                                     <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbProvinsi" CssClass="TextBox" Width="225px"/></td>    
                                     <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbKab" CssClass="TextBox" Width="225px"/></td>                                               
                                </tr>
                            </table>
                        </td>  
                                   
                </tr> 

                <tr>
                        <td></td>
                        <td>:</td>
                        <td colspan="7">
                            <table>
                                <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                    <td>Kecamatan</td>
                                    <td>Desa</td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbKec" CssClass="TextBox" Width="225px"/></td>
                                     <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbDesa" CssClass="TextBox" Width="225px"/></td>                 
                                </tr>
                            </table>
                        </td>                
                </tr>  

                <tr>
                    <td>No Peta Rincik</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPetaRincikNo" CssClass="TextBox" Width="225px"/></td>
                </tr> 

                 <tr>
                    <td>Jenis Dokumen</td>
                    <td>:</td>          
                    <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Width="230px" runat="server" ID="ddlJenisDok" >
                         <asp:ListItem Selected="True">AJB</asp:ListItem>
                                    <asp:ListItem>SPH</asp:ListItem>
                                    <asp:ListItem>SHM</asp:ListItem>
                        </asp:DropDownList>
                    </td>  
                   
                </tr> 

                 <tr>
                    <td>Nama Pembeli</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamaPembeli" CssClass="TextBox" Width="225px"/></td>
                </tr>  

                 <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" TextMode="MultiLine" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="450px"/></td>
                </tr>
            </table>
            <br />
            <hr style="color: Blue" />

            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Block" Value="0" ></asp:MenuItem>
                    <%--<asp:MenuItem Text="Equipment" Value="1" ></asp:MenuItem>--%>
                    <asp:MenuItem Text="Schedule Job" Value="2" ></asp:MenuItem>
                    <%--<asp:MenuItem Text="Schedule Job Detail" Value="3" ></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
            <br />
             
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="Tab2" runat="server">
                    
                    <asp:Panel ID="pnlDt" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
                       
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                 
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');"/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                     <asp:BoundField DataField="ItemNo" HeaderStyle-Width="150px" HeaderText="Item No"/>                                   
                                    <asp:BoundField DataField="KetKegiatan" HeaderStyle-Width="150px" HeaderText="Ket Kegiatan" />
                                    <asp:BoundField DataField="NoSurat" HeaderStyle-Width="150px" HeaderText="No Surat" />                            
                                     <asp:BoundField DataField="DateSurat" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                            HeaderStyle-Width="80px" SortExpression="DateSurat" 
                                            HeaderText="DateSurat">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                    
                                    <asp:BoundField DataField="Luas" HeaderStyle-Width="150px" HeaderText="Luas" />
                                    <asp:BoundField DataField="NameAkhir" HeaderStyle-Width="150px" HeaderText="Atas Nama Akhir" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                                    
                                </Columns>

                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />                      
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                   Item No</td>
                                <td>:</td>
                                <td colspan="4"> <asp:Label ID="lbItemNo" runat="server" Text="" /> </td>
                            </tr>

                             <tr>
                                <td>Ket Kegiatan</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbKetKegiatan" CssClass="TextBox" Width="225px"/></td>
                            </tr> 

                            <tr>
                                <td>No Surat</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoSurat" CssClass="TextBox" Width="225px"/></td>
                            </tr>

                             <tr>
                                <td>Tgl Surat</td>
                                <td>:</td>
                                <td>            
                                    <BDP:BasicDatePicker ID="tbDateSurat" runat="server" DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input" Width="225px"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="false" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
                                </td>
                            </tr>

                            <tr>
                                <td>Luas</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasDt" CssClass="TextBox" Width="225px"/></td>
                            </tr>

                            <tr>
                                <td>Nama Pemilik</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                        <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                            <td>Awal</td>
                                            <td>Akhir</td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAwal" CssClass="TextBox" Width="225px"/></td>
                                             <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAkhir" CssClass="TextBox" Width="225px"/></td>                 
                                        </tr>
                                    </table>
                                </td>                
                            </tr>  

                            <tr>
                                <td>
                                    Remark</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" 
                                        TextMode="MultiLine" Width="365px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

                <asp:View ID="Tab3" runat="server">
                        
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                                CommandName="Update" Text="Save" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                                CommandName="Cancel" Text="Cancel" /></EditItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="Equipment" HeaderText="Equipment" 
                                        SortExpression="Equipment" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="EquipmentName" HeaderStyle-Width="150px" 
                                        HeaderText="Equipment Name" SortExpression="EquipmentName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" 
                                        HeaderText="Qty" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" 
                                        HeaderText="Unit"
                                        ItemStyle-HorizontalAlign="Left" SortExpression="Unit" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                   <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                                        HeaderText="Remark" SortExpression="Remark" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt2ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                            ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Equipment</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEquip" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbEquipName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnEquip" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="65px" />
                                    <asp:Label ID="lblUnit" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                                <tr>
                                    <td>
                                        Remark
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbremarkEquip" runat="server" CssClass="TextBoxMulti" 
                                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                                        &nbsp; &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </tr>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

                <asp:View ID="Tab4" runat="server">
               
                    <asp:Panel ID="pnlDt3" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        
                            <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" /></EditItemTemplate></asp:TemplateField>
                                                
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btndetail" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="DetailMaterial" Text="Detail" Width="70" />
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                    
                                    <asp:BoundField DataField="NoWl" HeaderStyle-Width="100px" 
                                        HeaderText="No Wl" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="NoPercilDt2" HeaderStyle-Width="100px" 
                                        HeaderText="No Percil" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="AwalName" HeaderStyle-Width="150px" 
                                        HeaderText="Nama Awal" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LuasAwal" HeaderStyle-Width="150px" 
                                        HeaderText="Luas Awal"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="225px" 
                                        HeaderText="Remark" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                                                        
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                    <table>

                                    <tr>
                                        <td>No Wl</td>
                                        <td>:</td>
                                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbWlNo" CssClass="TextBox" Width="225px"/></td>
                                    </tr>
                                     <tr>
                                        <td>No Percil</td>
                                        <td>:</td>
                                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPercilNoDt" CssClass="TextBox" Width="225px"/></td>
                                    </tr>

                                    <tr>
                                    <td>Detail</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Nama Awal</td>
                                                <td>Luas Awal</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameAwal" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasAwal" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 



                            <tr>
                                    <td>Waris Level 1</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name Level 1</td>
                                                <td>Luas Level 1</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl1" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl1" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Waris Level 2</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name Level 2</td>
                                                <td>Luas Level 2</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl2" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl2" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Waris Level 3</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name Level 3</td>
                                                <td>Luas Level 3</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl3" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl3" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Waris Level 4</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name Level 1</td>
                                                <td>Luas Level 1</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl4" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl4" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Waris Level 5</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name Level 5</td>
                                                <td>Luas Level 5</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl5" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl5" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                           <td class="style1">
                                               Remark</td>
                                           <td class="style2">
                                               :</td>
                                           <td>
                                               <asp:TextBox ID="tbRemarkdt2" runat="server" CssClass="TextBox" Enabled="false" Height="38px" TextMode="MultiLine" Width="450px" />
                                           </td>
                                       </tr>
                    </table>

                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                
                 <asp:Panel ID="pnlInfoDt" runat="server">
                        <asp:Label ID="lblItem" runat="server" Text="No WL :" />
                        <asp:Label ID="lbNoWl" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="No Wl" />                                       
                            
                    </asp:Panel>
                       <br /> 
                    <asp:Panel runat="server" ID="PnlDt4">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt4" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                                
                        &nbsp;
                        <asp:Button ID="btnBackDt" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                            <br />
                            <br/>
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="NoDok" HeaderStyle-Width="100px" 
                                        HeaderText="No Dokumen" SortExpression="NoDok">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="NoAJB" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB" SortExpression="NoAJB">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name" HeaderStyle-Width="50px" 
                                        HeaderText="Name" SortExpression="Name">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Luas" ItemStyle-HorizontalAlign="Right" 
                                        SortExpression="Luas" />

                                    <asp:BoundField DataField="Sisa" HeaderStyle-Width="80px" HeaderText="Sisa" 
                                        SortExpression="Sisa">
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="50px" 
                                        HeaderText="Remark" SortExpression="Remark">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                        &nbsp;
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                        
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                        <table>
                            <tr>
                                <td>No Dokumen</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoDok" CssClass="TextBox" Width="225px"/></td>
                            </tr> 

                            <tr>
                                <td>Name</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt" CssClass="TextBox" Width="225px"/></td>
                            </tr> 

                            <tr>
                                <td>No AJB</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAJBSubDt" CssClass="TextBox" Width="225px"/></td>
                            </tr>  

                            <tr>
                                <td>Ukur</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                        <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                            <td>Luas M<sub>2</sub></td>
                                            <td>Sisa M<sub>2</sub></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox ID="tbLuasSubDt" runat="server" CssClass="TextBox" Width="100px"/></td>
                                            <td><asp:TextBox ID="tbSisaSubDt" ValidationGroup="Input" runat="server" CssClass="TextBox" width="100px"/></td>                                   
                                        </tr>
                                    </table>
                                </td>                
                            </tr>

                            <tr>
                                <td>Remark</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbremarkSubDt" CssClass="TextBox" Width="225px"/></td>
                            </tr> 

                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt4" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt4" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

            </asp:MultiView>
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New"
                ValidationGroup="Input" Width="90" />
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        </asp:Panel>
        
    </div>
   
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
