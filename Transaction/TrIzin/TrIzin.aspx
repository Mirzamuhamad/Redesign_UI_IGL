<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrIzin.aspx.vb" Inherits="Transaction_TrIzin_TrIzin" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <script type="text/javascript">  
        function openClosingdlg(_keyid, _prm) {  
                window.open("../../Transaction/TrIzin/FormComplete.Aspx?KeyId="+_keyid+"&ContainerId="+_prm+"Id","List","scrollbars=yes,resizable=no,width=700,height=500");
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
        try
         {         
//         var EstCost = document.getElementById("tbEstCost").value.replace(/\$|\,/g,"");                           
//         document.getElementById("tbEstCost").value = setdigit(EstCost,'<%=Viewstate("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }
        
        function validTime(source,args)
        {
            var res=false;
            var orgVAlue=args.Value;
            var hour=orgVAlue.substring(0,2);
            var min=orgVAlue.substring(4,5);
            if(orgVAlue.length==0)
                res=false;
            else if(orgVAlue.length>5)
                res=false;
            else
            {
                if((hour>23)||(min>59))
                    res=false;
                else
                    res=true;
            }
            args.IsValid=res;
        }
        
        function ExecRegex(obj)
        {
            var str=obj.value;
            var regex1= ([0-1]?[0-9]|2[0-3]):([0-5][0-9]);
            if(regex1.test(str))
            {return ;}
            //else 
            regex1= ([0-1]?[0-9]|2[0-3])([0-5][0-9]);
            if(regex1.test(str))
            {
                obj.value=RegExp.$1 + ":" +    RegExp.$2;
                //return ;
            }
            else
            {
                alert("Format error! \n The right format is: XX:XX");
                obj.value="XX:XX"
                //return ;
            }
        }
        
        function closing()
        {
            try
            {
                var result = prompt("Catatan Dokter", "", "Start Hour", "" );
                //var result2 = prompt("Start Hour", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                    //document.getElementById("HiddenStartHour").value = result2;
                    
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                    //document.getElementById("HiddenStartHour").value = "False Value";
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
    </head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    </div>
    <div class="Content">
    <div class="H1">Izin Istirahat / Pulang Awal</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Istirahat No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Istirahat Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                    
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Istirahat No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">istirahat Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                    
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" 
            Visible="False"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" Wrap="false" />
                  <RowStyle CssClass="GridItem" Wrap="false" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
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
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                              </asp:DropDownList>
                              <asp:Button ID="BtnGo" runat="server" class="btngo" 
                                  CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                                  Text="G" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" HeaderText="Istirahat No" SortExpression="Nmbr" />
                      <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Istirahat Date" htmlencode="true" SortExpression="TransDate" />
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" SortExpression="Remark" />
                  </Columns>
              </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table style="width: 538px">
        <tr>
            <td class="style6">Istirahat No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Istirahat Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
          <tr>
              <td class="style6">Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="386px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
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
                              <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                              <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField>
                              <ItemTemplate> 
                               <asp:Button Class="bitbtndt btngetitem" Width = "70" ID="btnClosing" runat="server" Text="Complete" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Closing" />
                              </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField>
                              <ItemTemplate> 
                               <asp:Button Class="bitbtndt btngetitem" Width = "85" ID="btnUnClosing" runat="server" Text="Un-Complete" CommandArgument='<%# Container.DataItemIndex %>' CommandName="UnClosing" />
                              </ItemTemplate>
                        </asp:TemplateField>   
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee No" />
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" />
                            <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="100px" HeaderText="Organization" />
                            <asp:BoundField DataField="StartHour" HeaderStyle-Width="80px" HeaderText="Start Hour" />
                            <asp:BoundField DataField="EndHour" HeaderStyle-Width="80px" HeaderText="End Hour" />
                            <asp:BoundField DataField="Keluhan" HeaderStyle-Width="250px" HeaderText="Alasan" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />                            
                            <asp:BoundField DataField="DoneComplete" HeaderStyle-Width="80px" HeaderText="Complete" />                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 90%">             
                    <tr>
                        <td>Employee</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbEmpNo" runat="server" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="116px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" MaxLength="255" ValidationGroup="Input" Width="325px" />
                            <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..." />
                        </td>
                    </tr>
                    <tr>                    
                        <td>Organization</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" Enabled="false" Height="18px" ValidationGroup="Input" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td>Start & End Time</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:TextBox ID="tbStartHour" runat="server" CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px"  />
                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="tbStartHour"
                                Mask="99:99"
                                MaskType="Time"
                                CultureName="en-us"
                                MessageValidatorTip="true"
                                runat="server">
                            </cc1:MaskedEditExtender>
                            &nbsp - &nbsp
                            <asp:TextBox ID="tbEndHour" runat="server" CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px" />
                            <cc1:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="tbEndHour"
                                Mask="99:99"
                                MaskType="Time"
                                CultureName="en-us"
                                MessageValidatorTip="true"
                                runat="server">
                            </cc1:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Keluhan</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:TextBox ID="tbKeluhan" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="360px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td style="margin-left: 40px"><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="360px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="margin-left: 40px">&nbsp;</td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenStartHour" runat="server" />
    <asp:HiddenField ID="HiddenEndHour" runat="server" />
    </form>
</body>
</html>
