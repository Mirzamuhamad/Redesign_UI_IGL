<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProduct.aspx.vb" Inherits="Master_MsProduct_MsProduct" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product File</title>
     <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
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
      
      
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    function setdigit(nStr, digit)
    {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        document.getElementById("tbWidth").value.replace(/\$|\,/g,"");
        document.getElementById("tbLength").value.replace(/\$|\,/g,"");
        document.getElementById("tbSquare2").value.replace(/\$|\,/g,"");
        
        var Width = document.getElementById("tbWidth");
        var Length = document.getElementById("tbLength");
        var Square = document.getElementById("tbSquare2");
        
        Square.value = parseFloat(Width.value) * parseFloat(Length.value);
     
        }catch (err){
            alert(err.description);
          }      
        }  
           
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value="Product_Name"></asp:ListItem> 
                  <asp:ListItem Text="Product Type Code" Value="ProductType_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Type Name" Value="ProductType_Name"></asp:ListItem>
                  <asp:ListItem Text="Product Group" Value="ProductGrp_Name"></asp:ListItem>
                  <asp:ListItem Text="Product SubGroup" Value="Product_Group"></asp:ListItem>
                  <asp:ListItem Text="Product SubGroup Name" Value="Product_GroupName"></asp:ListItem>
                  <%--<asp:ListItem Text="Product Jenis" Value="Product_Jenis"></asp:ListItem>
                  <asp:ListItem Text="Product Size" Value="Product_Size"></asp:ListItem>
                  <asp:ListItem Text="Product Bentuk" Value="Product_Bentuk"></asp:ListItem>
                  <asp:ListItem Text="Product Type" Value="Product_Type_Name"></asp:ListItem>
                  <asp:ListItem Text="Product Seri" Value="Product_Seri"></asp:ListItem>
                  <asp:ListItem Text="Product Merk" Value="Merk"></asp:ListItem>--%>
                  <asp:ListItem Text="Specification1" Value="Specification1"></asp:ListItem>
                  <asp:ListItem Text="Specification2" Value="Specification2"></asp:ListItem>
                  <asp:ListItem Text="Specification3" Value="Specification3"></asp:ListItem>
                  <asp:ListItem Text="Specification4" Value="Specification4"></asp:ListItem>
                  <asp:ListItem Text="Work Center" Value="WorkCenter"></asp:ListItem>
                  <asp:ListItem Text="Cost Center" Value="CostCtr"></asp:ListItem>
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                              
                  <asp:ListItem Text="Unit Order" Value="UnitOrder"></asp:ListItem>                  
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value="Product_Name"></asp:ListItem> 
                  <asp:ListItem Text="Product Type Code" Value="ProductType_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Type Name" Value="ProductType_Name"></asp:ListItem>
                  <asp:ListItem Text="Product Group" Value="ProductGrp_Name"></asp:ListItem>
                  <asp:ListItem Text="Product SubGroup" Value="Product_Group"></asp:ListItem>
                  <asp:ListItem Text="Product SubGroup Name" Value="Product_GroupName"></asp:ListItem>
                  <%--<asp:ListItem Text="Product Jenis" Value="Product_Jenis"></asp:ListItem>
                  <asp:ListItem Text="Product Size" Value="Product_Size"></asp:ListItem>
                  <asp:ListItem Text="Product Bentuk" Value="Product_Bentuk"></asp:ListItem>
                  <asp:ListItem Text="Product Type" Value="Product_Type_Name"></asp:ListItem>
                  <asp:ListItem Text="Product Seri" Value="Product_Seri"></asp:ListItem>
                  <asp:ListItem Text="Product Merk" Value="Merk"></asp:ListItem>--%>
                  <asp:ListItem Text="Specification1" Value="Specification1"></asp:ListItem>
                  <asp:ListItem Text="Specification2" Value="Specification2"></asp:ListItem>
                  <asp:ListItem Text="Specification3" Value="Specification3"></asp:ListItem>
                  <asp:ListItem Text="Specification4" Value="Specification4"></asp:ListItem>
                  <asp:ListItem Text="Work Center" Value="WorkCenter"></asp:ListItem>
                  <asp:ListItem Text="Cost Center" Value="CostCtr"></asp:ListItem>
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                              
                  <asp:ListItem Text="Unit Order" Value="UnitOrder"></asp:ListItem>              
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle Wrap="false" CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap = "false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				        <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="View" />
                                <asp:ListItem Text="Edit" />
                                <asp:ListItem Text="Copy New" />
                                <asp:ListItem Text="Photo" />
                                <asp:ListItem>Delete</asp:ListItem>                              
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %> "/>
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
				            <asp:BoundField DataField="Product_Code" SortExpression="Product_Code" HeaderText="Product Code"   />
				            <asp:BoundField DataField="Product_Name" SortExpression="Product_Name" HeaderText="Product Name" />
				            <asp:BoundField DataField="ProductType_Code" SortExpression="ProductType_Code" HeaderText="Product Type Code" />
				            <asp:BoundField DataField="ProductType_Name" SortExpression="ProductType_Name" HeaderText="Product Type Name" />
				            <asp:BoundField DataField="ProductCategory" SortExpression="ProductCategory" HeaderText="Product Category" />
				            <asp:BoundField DataField="Product_Group" SortExpression="Product_Group" HeaderText="SubGroup Code" />				            
				            <asp:BoundField DataField="Product_GroupName" SortExpression="Product_GroupName" HeaderText="SubGroup Name" />
				            <asp:BoundField DataField="ProductGrp_Name" SortExpression="ProductGrp_Name" HeaderText="Product Group" />
				            <asp:BoundField DataField="Merk" SortExpression="Merk" HeaderText="Product Merk" />
				            <asp:BoundField DataField="ProductDetail" SortExpression="ProductDetail" HeaderText="Product detail" />
				            <asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="Part No" />
				            <asp:BoundField DataField="Specification1" SortExpression="Specification1" HeaderText="Specification 1" />
				            <asp:BoundField DataField="Specification2" SortExpression="Specification2" HeaderText="Specification 2" />
				            <asp:BoundField DataField="Specification3" SortExpression="Specification3" HeaderText="Specification 3" />
				            <asp:BoundField DataField="Specification4" SortExpression="Specification4" HeaderText="Specification 4" />
				            <asp:BoundField DataField="FgStock" SortExpression="FgStock" HeaderText="Stock" />
				            <asp:BoundField DataField="FgProduce" SortExpression="FgProduce" HeaderText="Produce" />
				            <asp:BoundField DataField="Unit" SortExpression="Unit" HeaderText="Unit " />
				            <asp:BoundField DataField="UnitOrder" SortExpression="UnitOrder" HeaderText="Unit Order" />				            
				            <asp:BoundField DataField="Length" SortExpression="Length" HeaderText="Length" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" />
				            <asp:BoundField DataField="Width" SortExpression="Width" HeaderText="Width" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"/>
				            <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"/>
				            <asp:BoundField DataField="Volume" SortExpression="Volume" HeaderText="Volume" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"/>
				            <asp:BoundField DataField="MinQty" SortExpression="MinQty" HeaderText="Min Order" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"/>
				            <asp:BoundField DataField="ReorderQty" SortExpression="ReorderQty" HeaderText="ReOrder Qty" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right"/>
				            <asp:BoundField DataField="FgActive" SortExpression="FgActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center"/>							
						
    					</Columns>
        </asp:GridView>    
        </div>  
        <asp:Panel runat="server" ID ="pnlNav" Visible="false"> 
         <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
        </asp:Panel>								
      </asp:Panel>
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Product Code</td>
                <td>:</td>
                <td colspan="4"><asp:TextBox ID="tbCode" CssClass="TextBoxR" MaxLength="20" 
                        runat="server" Enabled ="false" ReadOnly="True" Width="201px"/></td>
                <td>Product Name</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbName" CssClass="TextBox" MaxLength="60" 
                        Width=225px runat="server" Height="24px" TextMode="MultiLine" />
                        <asp:Label ID="Label12" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
            </tr>
             <tr>
               <%-- <td>
                    Product Jenis</td>
                <td>
                    :</td>--%>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPJenis" Visible = "false" runat="server" AutoPostBack="True" 
                        CssClass="DropDownList" Width="205" />
                <%--</td>
                 <td>Product Type</td>
                <td>:</td>--%>
                <td>
                <asp:DropDownList ID="ddlPType" Width=205 runat="server" Visible = "false"  AutoPostBack="True"  CssClass="DropDownList"/>                   
                </td>
            </tr>
            <tr>
                <td>
                    Product Type</td>
                <td>
                    :</td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPMateri" AutoPostBack = "True" runat="server" 
                        CssClass="DropDownList" Width="205" Enabled="False" /> <asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label>
                </td>
          <%--      <td>Product Seri</td>
                <td>:</td>--%>
                <td>
                    <asp:DropDownList ID="ddlPSeri" Visible = "false"  runat="server" AutoPostBack="True" 
                        CssClass="DropDownList" Width="205" />
                </td>
            </tr>
            
             <tr>
                <td>Product Sub Group</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbPSubGroup" CssClass="TextBox" Width=50  MaxLength="6" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbPSubGroupName" CssClass="TextBox" MaxLength="60" Width=200 Enabled="False" runat="server" />
                    <asp:Button ID="btnPSubGroup" runat="server" class="btngo" Text="V"/>
                    <asp:Label ID="Label8" runat="server" ForeColor="Red">*</asp:Label>
                </td>
                
                <td>Product Detail / merk</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="ddlProductMerk" runat="server" AutoPostBack="True" 
                        CssClass="TextBox" Enabled="False" MaxLength="5" Width="225px" />
                        <asp:Label ID="Label13" runat="server" ForeColor="Red">*</asp:Label>
                </td>
            </tr>
            <tr>
                <td>Product Group</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbProductGroup" CssClass="TextBox" MaxLength="60" Width=200 Enabled="False" runat="server" />
                    <asp:Label ID="Label9" runat="server" ForeColor="Red">*</asp:Label>                    
                </td>
                
                <td>Part No</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbPartNo" runat="server" CssClass="TextBox" Enabled="False" 
                        MaxLength="30" Width="225" />
                </td>
            </tr> 
            
            
            <tr>
                <%--<td>
                    Product Size</td>
                <td>
                    :</td>--%>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPSize" Visible ="false" runat="server" AutoPostBack="True" 
                        CssClass="DropDownList" Width="205" />
                </td>
                 
            </tr>
            <tr>
                <%--<td>
                    Product Bentuk</td>
                <td>
                    :</td>--%>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPBentuk" Visible ="false" runat="server" AutoPostBack="True" 
                        CssClass="DropDownList" Width="205" />
                </td>
                
            </tr>
            <tr>
                <td>Work Center</td>
                <td>:</td>
                <td colspan="4"><asp:DropDownList CssClass="DropDownList" Width="205" ID="ddlWorkCtr" runat="server"/></td>                
                
                <td>Cost Ctr</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlCostCtr" runat="server" Width="230" CssClass="DropDownList" >                        
                    </asp:DropDownList>
                    <asp:Label ID="Label14" runat="server" ForeColor="Red">*</asp:Label>
                </td>
            </tr>
            <tr>
                <td>Specification 1</td>
                <td>:</td>
                <td colspan="4"><asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBox" ValidationGroup="Input" 
                      MaxLength="255" Width="200px" Height="43px" 
                      TextMode="MultiLine" /></td>
                 <td>Specification 2</td>
                <td>:</td>
                <td><asp:TextBox ID="tbSpecification2" runat="server" CssClass="TextBox" ValidationGroup="Input" 
                      MaxLength="255" Width="314px" Height="43px" 
                      TextMode="MultiLine" /></td>
            </tr>
            <tr>            
                <%--<td>Specification 3</td>
                <td>:</td>--%>
                <td colspan="4">
                    <asp:TextBox ID="tbSpecification3" Visible = "false" runat="server" CssClass="TextBox" 
                        Height="43px" MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" 
                        Width="314px" />
                </td>
                
                <%-- <td>
                    Specification 4</td>
                <td>
                    :</td>--%>
                <td>
                    <asp:TextBox ID="tbSpecification4" Visible = "false" runat="server" CssClass="TextBox" 
                        Height="43px" MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" 
                        Width="314px" />
                </td>
            </tr>
           
            <%--<tr>
                <td>Customer</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbCustCode" CssClass="TextBox" MaxLength="12" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbCustName" CssClass="TextBox" MaxLength="60" Width=200 Enabled="False" runat="server" />
                    <asp:Button ID="btnCustomer" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>     --%>           
                          
            
                        
            <tr>
                <td>Length x Width x Height</td>
                <td>:</td>
                <td colspan="4"><asp:TextBox ID="tbLength" CssClass="TextBox" runat="server" Width=100 /> m &nbsp x &nbsp 
                    <asp:TextBox ID="tbWidth" CssClass="TextBox" runat="server" Width=80/> m &nbsp x &nbsp 
                    <asp:TextBox ID="tbHeight" CssClass="TextBox" runat="server" Width=100/> mm
                </td>
                
            </tr>
            
            <tr>
                <td>Volume</td>
                <td>:</td>
                <td><asp:TextBox ID="tbSquare" CssClass="TextBoxR" Enabled="False" runat="server" Width="100"/> m3
                <%--<asp:TextBox ID="tbSquare2" CssClass="TextBox" Enabled="False" runat="server" Width=100/> --%>
                </td>
                <td>Weight</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbNetWeight" runat="server" CssClass="TextBox" Width="99px" />
                    Kg</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            
            
            <tr>
                <td>Qty Lot</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbQtyLot" runat="server" CssClass="TextBox" 
                        Width="100" />
                </td>
                <td>Qty Sample</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbGrossWeight" runat="server" CssClass="TextBox" Width="100" />
                </td>
                <td>
                    Percent Sample</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbpercentsample" runat="server" CssClass="TextBox" 
                        Width="100" />
                </td>
            </tr>
            
            <tr>
                <td>
                    Toleransi PO</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbTPO" runat="server" CssClass="TextBox" 
                        Width="101px" />
                    %</td>
                <td>
                    Toleransi RR</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbTRR" runat="server" CssClass="TextBox" 
                        Width="100" />
                    %</td>
                <td>
                    Toleransi TT</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbTolerance" runat="server" CssClass="TextBox" MaxLength="3" 
                        Width="100" />
                    %</td>
            </tr>
            <tr>
                <td>
                    Duty</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbduty" runat="server" CssClass="TextBox" 
                        Width="100" />
                    %</td>
                <td>
                    PPN BM</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbGramasi" runat="server" CssClass="TextBox" 
                        Width="100" />
                    %</td>
                <td>
                    COGS Price</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbCOGS" runat="server" CssClass="TextBox" MaxLength="3" 
                        Width="100" />
                    Rp</td>
            </tr>
            
            <tr>            
                <td>Min Qty</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbMinOrder" runat="server" CssClass="TextBox" Width="100" />
                </td>
                <td>Max Qty</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbQtyMax" runat="server" CssClass="TextBox" 
                        Width="100" />
                </td>
                <td>
                    Have Barcode</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlFgHaveBarcode" runat="server" CssClass="DropDownList" 
                        >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr> 
            
            <tr>
                <td>
                    Buffer Qty</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbQtyBuffer" runat="server" CssClass="TextBox" Width="100" />
                </td>
                <td>
                    Re Order Qty</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbMultiple" runat="server" CssClass="TextBox" Width="100" />
                </td>
                <td>
                    Have Part</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlFgHavePart" runat="server" CssClass="DropDownList">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Unit 1 (Warehouse)</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" 
                        Width="100px" />
                        <asp:Label ID="Label10" runat="server" ForeColor="Red">*</asp:Label>
                </td>
                <td>
                    Unit&nbsp; (Order)</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlUnitOrder" runat="server" CssClass="DropDownList" 
                        Width="100px" />
                        <asp:Label ID="Label11" runat="server" ForeColor="Red">*</asp:Label>
                </td>
                <td>
                    Packages</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlfgpackages" runat="server" CssClass="DropDownList">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Stock</td>
                <td>:</td>
                <td width="180px">
                    <asp:DropDownList ID="ddlFgStock" runat="server" Enabled=False CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Produce</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgProduce" runat="server" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    QC</td>
                <td>
                    :</td>
                <td>
                     <asp:DropDownList ID="ddlFgQC" runat="server" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            
            <tr>
                <td>Active</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgActive" runat="server" CssClass="DropDownList">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                 <td>Transfer</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgTransfer" runat="server" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td>Gift</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlfgGift" runat="server" CssClass="DropDownList">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
              
                
            </tr>   
              
            <tr>
                
               
                <%--<td>QA</td>
                <td>:</td>--%>
                <td>
                    <asp:DropDownList ID="ddlfgLiquid" Visible = "false" runat="server" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <%--<td>
                    Konsiyasi</td>
                <td>
                    :</td>--%>
                <td>
                    <asp:DropDownList ID="ddlFgKonsiyasi" Visible = "false" runat="server" CssClass="DropDownList">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                      <%--<td>Bibit</td>
                <td>:</td>--%>
                <td>
                    <asp:DropDownList ID="ddlfgbibit" Visible = "false" runat="server" CssClass="DropDownList">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>  
            <%--<tr>                
                <td>QC</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgQC" runat="server" AutoPostBack="True" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>QC Mikrobiologi</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgMikro" runat="server" CssClass="DropDownList" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>                        
            
                     
            <tr>
                <td>
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>
                </td>
                <td align="center" colspan="3">
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" />									
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
                </td>
            </tr>
        </table>
     </asp:Panel>   
     <asp:Panel ID="pnlDetail" runat="server" Visible = "false">       <br />
     <%--<asp:Label ID="lbDetailProduct" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />	 --%>
	 <%--<asp:Button Text="Back" ID="btnBackDtTop" Runat="server" CssClass="Button"/>--%>
	 <br />
	 <br />
	 <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Detail Bonus" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Convert" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Price" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Packages" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Same" Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Part" Value="5"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Customer" Value="6"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Supplier" Value="7"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Warehouse Area" Value="8"></asp:MenuItem>
                </Items>
            </asp:Menu>
            
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            
            <asp:View ID="Tab1" runat="server">
            
	               <asp:Label ID="label2" runat="server" CssClass="H1" Text="Bonus" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridBonus" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
							<asp:TemplateField HeaderText="Level Bonus" HeaderStyle-Width="80" SortExpression="LevelBonus">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="LevelBonus" text='<%# DataBinder.Eval(Container.DataItem, "LevelBonus") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="LevelBonusEdit" text='<%# DataBinder.Eval(Container.DataItem, "LevelBonus") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="LevelBonusAdd" CssClass="DropDownList" runat="server" DataSourceID="dsLevelBonus" Width="95%" 
                                        DataTextField="LevelName" DataValueField="LevelCode">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>																											
							
							<asp:TemplateField HeaderText="Percentage" HeaderStyle-Width="70" SortExpression="Percentage">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Percentage" text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PercentageEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PercentageAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>																																																								
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
            <asp:View ID="Tab2" runat="server">
            
	 <asp:ImageButton ID="btnBackDtTop" CommandName="Insert" runat="server" Visible="False" 
        ImageUrl="../../Image/btnBackOn.png"
        onmouseover="this.src='../../Image/btnBackOff.png';"
        onmouseout="this.src='../../Image/btnBackOn.png';"
        ImageAlign="AbsBottom" />
	            <br />
                <asp:Label ID="label1" runat="server" CssClass="H1" Text="Unit Convertion" />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
							<asp:TemplateField HeaderText="Unit Convert" HeaderStyle-Width="80" SortExpression="UnitConvert">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="UnitConvert" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="UnitConvertEdit" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="UnitConvertAdd" CssClass="DropDownList" runat="server" DataSourceID="dsUnitConvert" Width="95%" 
                                        DataTextField="Unit_Name" DataValueField="Unit_Code">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>																											
							
							<asp:TemplateField HeaderText="Rate" HeaderStyle-Width="70" SortExpression="Rate">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Rate" text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RateEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RateAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>																																																								
							<asp:TemplateField HeaderText="Unit" HeaderStyle-Width="80" SortExpression="UnitCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="UnitCode" text='<%# DataBinder.Eval(Container.DataItem, "UnitCode") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="UnitCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "UnitCode") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="UnitCodeAdd" CssClass="DropDownList" runat="server" DataSourceID="dsUnitConvert" Width="95%" 
                                        DataTextField="Unit_Name" DataValueField="Unit_Code">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>
							
							
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	 
                </asp:View>
            <asp:View ID="Tab3" runat="server">
            
	               <asp:Label ID="label3" runat="server" CssClass="H1" Text="Price" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridPrice" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
							<asp:TemplateField HeaderText="StartDate" HeaderStyle-Width="80" SortExpression="StartDate">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="StartDateEdit" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="LevelBonusAdd" CssClass="DropDownList" runat="server" DataSourceID="dsLevelBonus" Width="95%" 
                                        DataTextField="LevelName" DataValueField="LevelCode">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>																											
							
							<asp:TemplateField HeaderText="Percentage" HeaderStyle-Width="70" SortExpression="Percentage">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Percentage" text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PercentageEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PercentageAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>																																																								
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
            <asp:View ID="Tab4" runat="server">
            
	               <asp:Label ID="label5" runat="server" CssClass="H1" Text="Product Package" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridPackages" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
						
							<asp:TemplateField HeaderText="Item" HeaderStyle-Width="500px" SortExpression="ProductDt">
								<Itemtemplate>
									<asp:Label Runat="server" Width="25%" ID="ProductDt" text='<%# DataBinder.Eval(Container.DataItem, "ProductDt") %>'>
									</asp:Label>
									<asp:Label Runat="server" Width="65%" ID="ProductDtName" text='<%# DataBinder.Eval(Container.DataItem, "ProductDtName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductDtEdit" Width="25%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductDt") %>'
									ontextchanged="ProductDtEdit_TextChanged" AutoPostBack="true">
									</asp:TextBox>																		
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductDtNameEdit" Width="65%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductDtName") %>'>
									</asp:TextBox>	
									<asp:Button runat="server" ID="btnProductDtEdit" CommandName="btnProductDtEdit" CssClass="btngo" Text="..." />																	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductDtAdd" CssClass="TextBox" Width="25%" MaxLength="20" Runat="Server"
									ontextchanged="ProductDtAdd_TextChanged" AutoPostBack="true"/>	
									<asp:TextBox ID="ProductDtNameAdd" CssClass="TextBox" Width="65%" MaxLength="60" Runat="Server"/>	
									<asp:Button runat="server" ID="btnProductDtAdd" CommandName="btnProductDtAdd" CssClass="btngo" Text="..." />							    
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Item Part" HeaderStyle-Width="500px" SortExpression="ProductPart">
								<Itemtemplate>
									<asp:Label Runat="server" Width="25%" ID="ProductPart" text='<%# DataBinder.Eval(Container.DataItem, "ProductPart") %>'>
									</asp:Label>
									<asp:Label Runat="server" Width="65%" ID="ProductPartName" text='<%# DataBinder.Eval(Container.DataItem, "ProductPartName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductPartEdit" Width="25%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductPart") %>'
									ontextchanged="ProductPartEdit_TextChanged" AutoPostBack="true">
									</asp:TextBox>																		
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductPartNameEdit" Width="65%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductDtName") %>'>
									</asp:TextBox>	
									<asp:Button runat="server" ID="btnProductPartEdit" CommandName="btnProductPartEdit" CssClass="btngo" Text="..." />																	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductPartAdd" CssClass="TextBox" Width="25%" MaxLength="20" Runat="Server"
									ontextchanged="ProductPartAdd_TextChanged" AutoPostBack="true"/>	
									<asp:TextBox ID="ProductPartNameAdd" CssClass="TextBox" Width="65%" MaxLength="60" Runat="Server"/>	
									<asp:Button runat="server" ID="btnProductPartAdd" CommandName="btnProductPartAdd" CssClass="btngo" Text="..." />							    
								</FooterTemplate>
							</asp:TemplateField>			
																																																												
							<asp:TemplateField HeaderText="Qty" HeaderStyle-Width="70" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Unit" HeaderStyle-Width="70" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="UnitEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="UnitAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Percentage" HeaderStyle-Width="70" SortExpression="Percentage">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Percentage" text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PercentageEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PercentageAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="FgMain" HeaderStyle-Width="80" SortExpression="FgMain">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="FgMain" text='<%# DataBinder.Eval(Container.DataItem, "FgMain") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="FgMainEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgMain") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="FgMainAdd" CssClass="DropDownList" runat="server" Width="95%" >
								    <asp:ListItem Text="Y" Value="Y" ></asp:ListItem>                                              
                                    <asp:ListItem Text="N" Value="N" Selected="True" ></asp:ListItem>
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
            <asp:View ID="Tab5" runat="server">
            
	               <asp:Label ID="label4" runat="server" CssClass="H1" Text="Product Same" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridSame" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
						
							<asp:TemplateField HeaderText="Product Same" HeaderStyle-Width="500px" SortExpression="ProductSame">
								<Itemtemplate>
									<asp:Label Runat="server" Width="25%" ID="ProductSame" text='<%# DataBinder.Eval(Container.DataItem, "ProductSame") %>'>
									</asp:Label>
									<asp:Label Runat="server" Width="65%" ID="ProductSameName" text='<%# DataBinder.Eval(Container.DataItem, "ProductSameName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductSameEdit" Width="25%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSame") %>'
									ontextchanged="ProductSameEdit_TextChanged" AutoPostBack="true">
									</asp:TextBox>																		
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ProductSameNameEdit" Width="65%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSameName") %>'>
									</asp:TextBox>	
									<asp:Button runat="server" ID="btnSameEdit" CommandName="btnSameEdit" CssClass="btngo" Text="..." />																	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSameAdd" CssClass="TextBox" Width="25%" MaxLength="20" Runat="Server"
									ontextchanged="ProductSameAdd_TextChanged" AutoPostBack="true"/>	
									<asp:TextBox ID="ProductSameNameAdd" CssClass="TextBox" Width="65%" MaxLength="60" Runat="Server"/>	
									<asp:Button runat="server" ID="btnSameAdd" CommandName="btnSameAdd" CssClass="btngo" Text="..." />							    
								</FooterTemplate>
							</asp:TemplateField>																																																								
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
            <asp:View ID="Tab6" runat="server">
            
	               <asp:Label ID="label6" runat="server" CssClass="H1" Text="Part" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridPart" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
						
							<asp:TemplateField HeaderText="Part No" HeaderStyle-Width="100px" SortExpression="PartNo">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="PartNo" text='<%# DataBinder.Eval(Container.DataItem, "PartNo") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PartNoEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PartNo") %>'>
									</asp:TextBox>																		
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PartNoAdd" CssClass="TextBox" Width="100%" MaxLength="20" Runat="Server"/>	
								</FooterTemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Part Name" HeaderStyle-Width="500px" SortExpression="PartName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="PartNo" text='<%# DataBinder.Eval(Container.DataItem, "PartName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PartNameEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PartName") %>'>
									</asp:TextBox>																		
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PartNameAdd" CssClass="TextBox" Width="100%" MaxLength="20" Runat="Server"/>	
								</FooterTemplate>
							</asp:TemplateField>																																																								
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
				
				
             
             <asp:View ID="Tab12" runat="server">
            
	               <asp:Label ID="label7" runat="server" CssClass="H1" Text="Warehouse Area" />
	 <br />
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="GridWrhsArea" runat="server" ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            
							<asp:TemplateField HeaderText="Warehouse Area" HeaderStyle-Width="80" SortExpression="WrhsArea">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="WrhsArea" text='<%# DataBinder.Eval(Container.DataItem, "WrhsArea") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="WrhsAreaEdit" text='<%# DataBinder.Eval(Container.DataItem, "WrhsArea") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="WrhsAreaAdd" CssClass="DropDownList" runat="server" DataSourceID="dsWrhsArea" Width="95%" 
                                        DataTextField="Wrhs_Area_Name" DataValueField="Wrhs_Area_Code">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							    <HeaderStyle Width="80px" />
							</asp:TemplateField>																											
							
							<asp:TemplateField HeaderText="QtyMin" HeaderStyle-Width="70" SortExpression="QtyMin">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="QtyMin" text='<%# DataBinder.Eval(Container.DataItem, "QtyMin") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyMinEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyMin") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyMinAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>

																																																								
							<asp:TemplateField HeaderText="QtyMax" HeaderStyle-Width="70" SortExpression="QtyMax">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="QtyMax" text='<%# DataBinder.Eval(Container.DataItem, "QtyMax") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyMaxEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyMax") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyMaxAdd" CssClass="TextBox" Width="90%" MaxLength="10" Runat="Server"/>								    
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Transfer Type" HeaderStyle-Width="80" SortExpression="TransferType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="TransferType" text='<%# DataBinder.Eval(Container.DataItem, "TransferType") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								     <asp:DropDownList ID="TransferTypeEdit" CssClass="DropDownList" runat="server" >

                                          <asp:ListItem Value="OPEN">OPEN</asp:ListItem>
                                        <asp:ListItem Value="KEEP MAX">KEEP MAX</asp:ListItem>
                                        <asp:ListItem Value="KEEP MIN">KEEP MIN</asp:ListItem>

                                    </asp:DropDownList>	
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="TransferTypeAdd" CssClass="DropDownList" runat="server" >

                                        <asp:ListItem Value="OPEN">OPEN</asp:ListItem>
                                        <asp:ListItem Value="KEEP MAX">KEEP MAX</asp:ListItem>
                                        <asp:ListItem Value="KEEP MIN">KEEP MIN</asp:ListItem>

                                    </asp:DropDownList>								    
								</FooterTemplate>											
							    <HeaderStyle Width="80px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>
							    <HeaderStyle Width="126px" />
                                <ItemStyle Wrap="False" />
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	   
                </asp:View>
            </asp:MultiView>
            <br />            
            
            
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    <asp:SqlDataSource ID="dsLevelBonus" runat="server" SelectCommand="EXEC S_GetLevelBonus"></asp:SqlDataSource>     
    <asp:SqlDataSource ID="dsUnitConvert" runat="server" SelectCommand="EXEC S_GetUnit"></asp:SqlDataSource>     
    <asp:SqlDataSource ID="dsWrhsArea" runat="server" SelectCommand="EXEC S_GetWrhsArea"></asp:SqlDataSource>  
    
      <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
           
    </form>
</body>
</html>
