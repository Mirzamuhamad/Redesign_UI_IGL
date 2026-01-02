<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormCekPostPR.Aspx.vb" Inherits="FormCekPostPR" %>

<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Form</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content"> <%--nambahin ini doank--%>
        <asp:Panel runat="server" ID="pnlDt">
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="Gridcek" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>               
                        <asp:BoundField DataField="Department" HeaderStyle-Width="250px" 
                            HeaderText="Department" >                        
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Budget" HeaderText="Budget" />                        
                        <asp:BoundField DataField="Product" HeaderStyle-Width="250px" 
                            HeaderText="Product" >                                                                        
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyBudget" HeaderText="Qty Budget" />    
                        <asp:BoundField DataField="QtyUse" HeaderText="Qty Use" />    
                        <asp:BoundField DataField="QtyPR" HeaderText="Qty PR" />    
                        <asp:BoundField DataField="QtyBalance" HeaderText="Qty Balance" />    
                        <asp:BoundField DataField="Description" HeaderStyle-Width="250px" 
                            HeaderText="Description" >                        
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
       <asp:Panel runat="server" ID="pnlEditDt" Visible="False">
            <table>
                <tr>
                    <td>BAP No.</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" Width ="150" ID="tbBAPNo" />                                              
                     </td>
                </tr>
                <tr>
                    <td>BAP Date</td>
                    <td>:</td>
                    <td>
                    <BDP:BasicDatePicker ID="tbBAPDate" runat="server" DateFormat="dd MMM yyyy" 
                                ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                DisplayType="TextBoxAndImage" 
                                TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>            
                </tr>                      
                <tr>
                    <td>Save to posting</td>
                    <td>:</td>
                </tr>                                                                          
            </table>
            <br />                     
       </asp:Panel>           
            <asp:ImageButton ID="btnOK2" runat="server"  
                 ImageUrl="../../Image/btnOKOn.png"
                 onmouseover="this.src='../../Image/btnOKOff.png';"
                 onmouseout="this.src='../../Image/btnOKOn.png';"
                 ImageAlign="AbsBottom" />
            <asp:ImageButton ID="btnCancel2" runat="server"  
                 ImageUrl="../../Image/btnCancelOn.png"
                 onmouseover="this.src='../../Image/btnCancelOff.png';"
                 onmouseout="this.src='../../Image/btnCancelOn.png';"
                 ImageAlign="AbsBottom" />
            <asp:ImageButton ID="btnBack2" runat="server"  
                 ImageUrl="../../Image/btnBackOn.png"
                 onmouseover="this.src='../../Image/btnBackOff.png';"
                 onmouseout="this.src='../../Image/btnBackOn.png';"
                 ImageAlign="AbsBottom" />                 
       </asp:Panel>          
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
