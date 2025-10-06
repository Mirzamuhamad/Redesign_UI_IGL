<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSJPks.aspx.vb" Inherits="Transaction_TrSJPks" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SJ Pks (Surat Jalan PKS)</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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

        function setformat()
        {
        
         try
         {           
        document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');        
        }catch (err){
            alert(err.description);
          }      
        }   
        function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">SJ PKS</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="No_SPTOut">SPTOut No</asp:ListItem>
                      <asp:ListItem Value="No_Timbang">Timbang No</asp:ListItem>
                      <asp:ListItem Value="Customer">Customer</asp:ListItem>
                      <asp:ListItem Value="JamMasuk">Jam Masuk</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>  
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                          
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
            </td>

            <td>
                &nbsp;</td>
                <td>
                    &nbsp;</td>

            <td>
                Scan slip Timbang</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbScanSlip" Autopostback="True" runat="server" CssClass="TextBox" Width="150px" />
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
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>            
                       <asp:ListItem Value="No_SPTOut">SPTOut No</asp:ListItem>
                      <asp:ListItem Value="No_Timbang">Timbang No</asp:ListItem>
                      <asp:ListItem Value="Customer">Customer</asp:ListItem>
                      <asp:ListItem Value="JamMasuk">Jam Masuk</asp:ListItem>  
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>               
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
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
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
							  <asp:ListItem Text="Print Lampiran" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="Reference" SortExpression="Reference" 
                      HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                  <asp:BoundField DataField="No_SPTOut" HeaderStyle-Width="120px" HeaderText="No SPTOut" 
                      SortExpression="No_SPTIN">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="No_Timbang" HeaderStyle-Width="120px" HeaderText="No Timbang" 
                      SortExpression="No_Timbang">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                   <asp:BoundField DataField="Customer" HeaderStyle-Width="120px" HeaderText="Customer" 
                      SortExpression="Customer">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="CustomerName" HeaderStyle-Width="120px" HeaderText="Customer Name" 
                      SortExpression="CustomerName">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="JamMasuk" HeaderStyle-Width="120px" HeaderText="JamMasuk" 
                  SortExpression="JamMasuk">
                </asp:BoundField>

                  <asp:BoundField DataField="JamKeluar" HeaderStyle-Width="120px" HeaderText="JamKeluar" 
                  SortExpression="JamKeluar">                 
              </asp:BoundField>
                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	     
            &nbsp &nbsp &nbsp  

            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />          
                        
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td>            
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>   
            
            <td>
                Customer</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbCustomer" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCustomerName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="200px" />
                <asp:Button ID="btnCustomer" runat="server" Class="btngo" Text="..." />
            </td>
            
        </tr>      
        <%--<tr>
            <td>
            <asp:ImageButton ID="btnGetDt" ValidationGroup="Input" runat="server"  
                    ImageUrl="../../Image/btnGetDataOn.png"
                    onmouseover="this.src='../../Image/btnGetDataOff.png';"
                    onmouseout="this.src='../../Image/btnGetDataOn.png';"
                    ImageAlign="AbsBottom" />             
            </td>            
        </tr>--%>
        <tr>
              <td>No Slip SptOut</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbNoSptOut" runat="server" CssClass="TextBox" />
              </td>

              <td>
                Car</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbCar" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCarName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="200px" />
                <asp:Button ID="btnCar" runat="server" Class="btngo" Text="..." />
            </td>

              

           
              
          </tr>

          <tr>
            <td>
                No Slip Timbang</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbNoTimbang" runat="server" CssClass="TextBox" />
            </td>


            <td>
                Operator</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbOperatorName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="200px" />
                <asp:Button ID="btnOperator" runat="server" Class="btngo" Text="..." />
            </td>
            

            <%--
            <td>
                Scan slip Timbang</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbScanSlip" Autopostback="True" runat="server" CssClass="TextBox" Width="150px" />
            </td>
            --%>
          </tr>

          <tr>
            <td>
                Jam Masuk</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbJamMasuk" runat="server" CssClass="TextBox" 
                    />
            </td>

            <td>
                Jam Keluar</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbJamkeluar" runat="server" CssClass="TextBox" 
                    />
            </td>

            
          </tr>
          
        <tr>
            <td>Remark</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                        ValidationGroup="Input" Width="250px" />
                </td>                
            </td>              
          </tr>
          
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                               <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
						       <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" enabled = "False" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                            </ItemTemplate>
                            <EditItemTemplate>
                            		<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																											
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 													
                                
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderStyle-Width="100px" HeaderText="Item" SortExpression="ItemNo" ></asp:BoundField>
                        <asp:BoundField DataField="So_No" HeaderStyle-Width="80px" HeaderText="No So" ></asp:BoundField>
                        <asp:BoundField DataField="Do_No" HeaderStyle-Width="80px" HeaderText="No Do" ></asp:BoundField>
                        <asp:BoundField DataField="ItemType" HeaderStyle-Width="80px" HeaderText="ItemType" ></asp:BoundField>
                        <asp:BoundField DataField="ItemTypeName" HeaderStyle-Width="80px" HeaderText="Item Type Name" ></asp:BoundField>
                        <asp:BoundField DataField="ContractNo" HeaderStyle-Width="80px" HeaderText="No Kontrak" ></asp:BoundField>
						<asp:BoundField DataField="NoSegel" HeaderStyle-Width="80px" HeaderText="No Segel" ></asp:BoundField>
                        <asp:BoundField DataField="Shipper" HeaderStyle-Width="80px" HeaderText="Shipper" ></asp:BoundField>
                        <asp:BoundField DataField="SisaContract" HeaderText="Sisa Kontrak" SortExpression="SisaContract" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Timbang1" HeaderText="Timbang 1" SortExpression="Timbang1" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Timbang2" HeaderText="Timbang 2" SortExpression="Timbang2" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Netto1" HeaderText="Netto1" SortExpression="Netto1" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Potongan" HeaderText="Potongan" SortExpression="Potongan" DataFormatString="{0:#,##0.00}"></asp:BoundField>                        
                        <asp:BoundField DataField="Netto2" HeaderText="Netto2" SortExpression="Netto2" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" ></asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
 
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>Item No</td>
                            <td>:</td>
                            <td><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                            </td>           
                       </tr> 
                       <tr>
                           <td>SO No</td>
                           <td>:</td>
                           <td><asp:textBox ID="tbSo" runat="server" cssClass="TextBox" ValidationGroup="Input" Width="91px"/></td>
                       </tr>
                       <tr>
                        <td>Do No</td>
                        <td>:</td>
                        <td><asp:textBox ID="tbDo" runat="server" cssClass="TextBox" ValidationGroup="Input" Width="91px"/></td>
                    </tr>
                       <tr>
                        <td>No Kontrak</td>
                        <td>:</td>
                        <td><asp:textBox ID="tbKontrak" runat="server" cssClass="TextBox" ValidationGroup="Input" Width="91px"/></td>
                    </tr>
                    <tr>
                        <td>Item Type</td>
                        <td>:</td>
                        <td><asp:textBox ID="tbItemType" runat="server" cssClass="TextBox" ValidationGroup="Input" Width="91px"/></td>
                    </tr>
                    <tr>
                        <td>Sisa Kontrak</td>
                        <td>:</td>
                        <td><asp:textBox ID="tbSisaKontrak" runat="server" cssClass="TextBox" ValidationGroup="Input" Width="91px"/></td>
                    </tr>
                        <tr>
                            <td>
                                Nominal</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                            Timbang 1</td>
                                        <td>
                                            Timbang 2</td>
                                        <td>
                                            Netto 1</td>
                                        <td>
                                            Potongan </td>
                                        <td>
                                            Netto 2</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbTimbang1" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTimbang2" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="tbNetto1" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="tbPotongan" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>
                                        
                                        <td>
                                            <asp:TextBox ID="tbNetto2" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
						<tr>
                            <td>
                                No Segel
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbNoSegel" runat="server" CssClass="TextBox" Width="150px" 
                                    />
                            </td>
                        </tr>
						<tr>
                            <td>
                                Shipper
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbShipper" runat="server" CssClass="TextBox" Width="150px" 
                                     />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" 
                                    Height="31px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
                	&nbsp;</td>
            </tr>
       </table>
            <br />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																											
									
			<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> 
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
   <%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%>
      <br />             
    </asp:Panel>               
    </div>   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />    
    </form>                            
    </body>
</html>
