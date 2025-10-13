<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDocReceive.aspx.vb" Inherits="Transaction_TrDocReceive_TrDocReceive" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dokumen Receive</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>

    
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
        
        




 


      function UploadInvoice(fileUploadInvoice) {
          if (fileUploadInvoice.value != '') {
              document.getElementById("<%=btnSaveINV.ClientID %>").click();
          }
      }

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
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Dokumen Receive</div>
     <hr />        
     <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>

                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                 
                 
                
            </td>
            
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 <%--&nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding SPK: "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X"  Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>--%>
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
          <td>
           
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
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
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
            
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="120px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField> 
                  </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                      
          </asp:Panel> 
     </asp:Panel>        
     
 
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>
                     <asp:Menu ID="Menu2" runat="server" CssClass="Menu" ItemWrap="False" 
                Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
                StaticMenuItemStyle-CssClass="MenuItem" 
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Form Input" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Upload Dokumen Receive" Value="1"></asp:MenuItem>
                </Items>
            </asp:Menu>
                    
              </td>
                <td>
                <asp:Button class="bitbtndt btnback" Visible = "false" runat="server" ID="btnGoEdit" Text="Back" /> 
                </td>
            </tr>
        </table>
        
         <br /> 
    <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
       <asp:View ID="TabHd0" runat="server">     
          <table>
            <tr>
                <td>Transaction No</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="225px" Enabled="False"/>        
                </td>   
                
                  
            </tr>
            <tr>
            <td>Transaction Date</td>
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

           <%-- <tr>
           
                 <td>Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" CssClass="TextBox" Width="50px"/>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAreaName" CssClass="TextBox" Width="166px"/>
                <asp:Button Class="btngo" ID="btnArea" Text="..." runat="server" ValidationGroup="Input" /></td>

            </tr>--%>

            <tr>
                <td>Remark</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" Width="400px" TextMode="MultiLine" MaxLength="255"/></td>
                
            </tr>
          </table> 
          
       </asp:View>
       
       <asp:View ID="TabHd1" runat="server">
            <table>
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearInv" Width="15px" Text="s" /> Upload Dokumen Receive </td>
                        <td>:</td>
                  
                  <td> 
                     <asp:FileUpload runat="server" style="color: White;" accept="application/pdf" ID="FubInv"  />
                     <asp:Button ID="btnsaveINV" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />               
                  </td> 
                  <td>        
                    <asp:LinkButton ID="lbDokInv" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" />--%> 
                  </td>           
                </tr>
               
            </table>
       </asp:View>
       
    </asp:MultiView> 
 
 
      <br />
      <asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                 <asp:MenuItem Text="Detail Dokumen" Value="0"></asp:MenuItem> 
                <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
       

            
       <br />
      
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="Tab1" runat="server">
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
           	
            
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="False"  Wrap="false" >
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
                         
                        </asp:TemplateField>
                        
                            <asp:BoundField DataField="ItemNo" HeaderText="No Item" />
                            <asp:BoundField DataField="NoDoc" HeaderStyle-Width="100px" HeaderText="No Dokumen" />
                            <asp:BoundField DataField="IjinName" HeaderStyle-Width="100px" HeaderText="Type Dokumen" /> 
                             
                            <%--<asp:BoundField DataField="PaymentNo" HeaderStyle-Width="100px" HeaderText="PaymentNo" />--%>  
                            <asp:BoundField DataField="Perihal" HeaderStyle-Width="150px" HeaderText="Perihal" />                          
                            
                            <asp:BoundField DataField="TerbitDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="120px" SortExpression="TerbitDate" HeaderText="Tanggal Terbit"></asp:BoundField>
                            <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="120px" SortExpression="EndDate" HeaderText="Masa berlaku s/d"></asp:BoundField>
                            
                            
                            <asp:BoundField DataField="Instansi" HeaderText="Penerbit"  HeaderStyle-Width="100px" />                           
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                                
                
                <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="Item" />
                        </td>           
                    </tr>
                    
                     <tr> 
                     
                       <td>Type Ijin</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlType" ValidationGroup="Input" AutoPostBack="true" Width="230px" runat="server" CssClass="DropDownList" /> 
                                                                                         
                            </td>                
                    </tr>  
                    
                     <tr>
                        <td>No Dokumen </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbNoDok" CssClass="TextBox" Width="225px" 
                                MaxLength="255"  />                        
                        </td>
                    </tr>
                    
                    <tr>                    
                              <%--  <td>Payment No</td>
                                <td>:</td>--%>
                                <td>                             
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentNo"  Visible="false" Width="225px"/> 
                                     <asp:TextBox runat="server" ID="tbFgValueDt2" Visible="false" />
                                     <%--   <asp:Label ID="lbItemNo5" runat="server" Text="*" />  --%>                                       
                                </td>
                    </tr>   
                    
                    
                    <tr>
                        <td>Perihal </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbPerihal" CssClass="TextBox" Width="225px" 
                                MaxLength="255"  />                        
                        </td>
                    </tr>
                      
                     
                   <%-- <tr>                    
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="TbUnit" CssClass="TextBox" Width="50px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbUnitName" CssClass="TextBox" Width="175px"/>
                        <asp: Class="btngo" ID="btnUnit" Text="..." runat="server" ValidationGroup="Input" /></td>
                    </tr>--%>
                    
                    
                  <tr>
                        <td>Instansi Penerbit </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbPenerbit" CssClass="TextBox" Width="225px" 
                                MaxLength="255"  />                        
                        </td>
                    </tr>  
                  <tr>  
                     <td>Tanggal Terbit</td>
                        <td>:</td>
                        <td>    
                            <BDP:BasicDatePicker ID="tbdateTerbit" runat="server" DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
                        </td> 
                </tr>
                
                 <td>Masa Berlaku s/d</td>
                <td>:</td>
                <td>    
                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>            
                </td> 
 
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                                      
            </table>
            <br />    
            
    		<asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									

        </asp:Panel> 
       </asp:View>
                
                
       <asp:View ID="Tab2" runat="server">
             <asp:Panel ID="pnlDt2" runat="server">
                                 
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                         <asp:Button Class="bitbtndt btnsearch" ID="btnGetDt" Text="Get Data" runat="server" Visible="false" ValidationGroup="Input" />                                 	
             
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" /></EditItemTemplate></asp:TemplateField>                                                
                                  
                                     <asp:BoundField DataField="PaymentNo" HeaderStyle-Width="150px" 
                                        HeaderText="Payment No" SortExpression="PaymentNo" ><HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                                        HeaderText="Remark" SortExpression="Remark" ><HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />                        
                    </asp:Panel>
                    
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    No Payment</td>
                                <td> : </td>
                                <td>
                                    <asp:TextBox ID="tbPaymentNoDt" Width = "225px" runat="server" Enabled ="True"
                                        CssClass="TextBox"  />
                                        <asp:Button Class="btngo" ID="btnPaymentNo" Text="..." runat="server" />
                                 </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Remark</td>
                                <td> : </td>
                                <td>
                                    <asp:TextBox ID="tbRemarkdt3" Width = "225px" TextMode = "MultiLine" runat="server" Enabled ="True"
                                        CssClass="TextBox"  />
                                 </td>
                            </tr>

                        </table>
                        
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
       </asp:View>       
      </asp:MultiView>
       <br />   
       
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
       
      
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">

      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
   
    </form>
    </body>
</html>
