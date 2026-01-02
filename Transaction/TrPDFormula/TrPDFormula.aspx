<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPDFormula.aspx.vb" Inherits="Transaction_TrPDFormula_TrPDFormula" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
	    
	    z = (x1 + x2).replace(/\$|\,/g,"");
	    	    
	    if (isNaN(z) == true)
        {
           return 0;
        }    
	              
	    return x1 + x2;
            
	    
	    }catch (err){
            alert(err.description);
          }  
        }
        
        function cekNan(nstr)
        {
            if(isNaN(nstr) == true)
            {
                return 0;
            }
            return nstr;
        }
     
        
        function setformatdt()
        {
            var Perc = document.getElementById("tbStandard").value.replace(/\$|\,/g,""); 
         try
         {                       
            document.getElementById("tbStandard").value = setdigit(Perc,-1);
        }catch (err){
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
    <div class="H1">Formula Material </div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value ="TransNmbr" Selected="True">Formula No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem Value="FormulaName">Formula Name</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Formula Date</asp:ListItem>
                      <asp:ListItem Value="Effective_Date">Effective Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      
                      <%--<asp:ListItem Value="ProcessName">Process</asp:ListItem>--%>
                      <asp:ListItem>Remark</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />    
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
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
                      <asp:ListItem Value ="TransNmbr" Selected="True">Formula No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem Value="FormulaName">Formula Name</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Formula Date</asp:ListItem>
                      <asp:ListItem Value="Effective_Date">Effective Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      
                      <%--<asp:ListItem Value="ProcessName">Process</asp:ListItem>--%>
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
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="false"> 
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
                              <%--<asp:ListItem Text="Print" />--%>
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Copy New" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" SortExpression="TransNmbr" HeaderText="Formula No"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Trans_Date" SortExpression="Trans_Date" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Effective_Date" SortExpression="Effective_Date" HeaderText="Eff Date"></asp:BoundField>                  
                  <asp:BoundField DataField="FormulaName" SortExpression="FormulaName" HeaderStyle-Width="220px" HeaderText="Formula Name" />                  
                  <asp:BoundField DataField="Remark" sortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>Formula No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ReadOnly="true" ID="tbCode" Width="150px"/>
            </td>           
            
            <td>Revisi</td>
            <td>:</td>
            <td><asp:Label runat="server" ID="lbRevisi"></asp:Label></td>
        </tr>      
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
            <td>Effective Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbEffDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        
        </tr>            
        <%--<tr>
            <td>Formula Type</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlFormulaType" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input">
                    <asp:ListItem Selected="True">Produksi</asp:ListItem>
                    <asp:ListItem>Testing</asp:ListItem>
                </asp:DropDownList>                
            </td>                    
        </tr>--%> 
        <tr>
            <td>Formula Name</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "250px" runat="server" ValidationGroup="Input" 
                    ID="tbFormulaName" CssClass="TextBox" MaxLength="100" />
                    <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label></td>
                    
        </tr>               
        
        <%--<tr>
            <td>For All Motif</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlForAllMotif" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>                
            </td>                    
        </tr>--%>        
        
       <%--<tr>            
            <td> Process </td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlProcess" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="180px" />                    
               <asp:Button class="bitbtn btngo" ValidationGroup="Input" runat="server" 
                    ID="btnGetDt" Text="Get Data" Visible="false" Width="74px"/>    
            </td>
        </tr>--%>     
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" Width="365px" />
                <asp:Button ID="btnGetDt" runat="server" Class="bitbtndt btngetitem" 
                    Text="Get Data" ValidationGroup="Input" width="100px" />
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />
        <asp:Menu
            ID="Menu1"
            Width="100%"
            runat="server"
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"    
            Orientation="Horizontal"            
            StaticEnableDefaultPopOutImage="False">
            
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            
            <Items>
                <asp:MenuItem Text="Detail Formula" Value="0"></asp:MenuItem>                
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                                                 
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
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
                            <asp:BoundField DataField="Material" HeaderStyle-Width="120px" HeaderText="Material" />
                            <asp:BoundField DataField="MaterialName" HeaderStyle-Width="250px" HeaderText="Material Name" />
                            <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" FooterStyle-HorizontalAlign="Right"  HeaderText="Specification" />
                            <asp:BoundField DataField="Percentage" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  HeaderText="Percentage (%)" />
                            <%--<asp:BoundField DataField="Standard" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Komposisi (%)" />
                            <asp:BoundField DataField="Adjustment" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Adjustment (%)" />--%>
                        </Columns>
                    </asp:GridView>
              </div>   
             <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	                
              
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Material</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbMaterial" CssClass="TextBox" 
                                AutoPostBack="true" />
                            <asp:TextBox runat="server"  CssClass="TextBoxR"
                                ID="tbMaterialName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="250px"/> 
                        <asp:Button Class="btngo" ID="btnMaterial" Text="..." runat="server" ValidationGroup="Input" />                                                                     
                                                                           
                        </td>           
                    </tr>        
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxMulti" Enabled ="false" 
                                MaxLength="255" TextMode="MultiLine" Width="365px" />                            
                        </td>           
                    </tr>        
                    <tr>
                        <td>Percentage (%)</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server"  CssClass="TextBox"
                                ID="tbStdKadarAir" EnableTheming="True" ReadOnly="False" Enabled="True" 
                                Width="50px"/>
                            <asp:Label runat="server"  id="lbPercent1" Text ="%"/>                                                            
                        </td>           
                    </tr>      
                    <%--<tr>
                        <td>Standard Komposisi</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                                ID="tbStandard" Width="50px" AutoPostBack="True"/>                        
                            <asp:Label runat="server"  id="lbPercent2" Text ="%"/>                                                            
                        </td>           
                    </tr> --%>     
                    
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />	
           </asp:Panel> 
              
           </asp:View>
           
                        
        </asp:MultiView>
    
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	  
    </asp:Panel>
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
     <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />
    </form>
</body>
</html>
