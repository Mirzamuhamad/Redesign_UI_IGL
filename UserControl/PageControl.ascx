<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PageControl.ascx.vb" Inherits="UserControl_PageControl" %>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
        <asp:panel runat="server" BorderStyle="NotSet" BorderWidth="1" style="width:210px; height: 20px;">        
        &nbsp;
        <asp:Label ID="Label1" Text = "Page : " Font-Size="Small" runat="server" ></asp:Label>            
        <asp:TextBox ID="tbpageNo" Width="30px" Text = "1" Font-Size="X-Small" AutoPostBack="true" runat="server"></asp:TextBox>        
        <asp:Label ID="lbPageof" Text = "of" Font-Size="Small" runat="server" ></asp:Label>
        <asp:Label ID="lbpageMax" Width="40px" Text = "1" Font-Size="Small" Font-Bold="true" runat="server"> </asp:Label>                
        <asp:ImageButton ID="btnFirst" runat="server" ToolTip="First Page" 
                    ImageUrl="../Image/bbAddAll.bmp"                    
                    ImageAlign="AbsMiddle" Visible="true" />    
        <asp:ImageButton ID="btnPrev" runat="server"  ToolTip="Previous Page"
                    ImageUrl="../Image/bbAdd.bmp"                    
                    ImageAlign="AbsMiddle" Visible="true" />  
        <asp:ImageButton ID="btnNext" runat="server"  ToolTip="Next Page"
                    ImageUrl="../Image/bbRemove.bmp"                    
                    ImageAlign="AbsMiddle" Visible="true" /> 
        <asp:ImageButton ID="btnLast" runat="server"  ToolTip="Last Page"
                    ImageUrl="../Image/bbRemoveAll.bmp"                    
                    ImageAlign="AbsMiddle" Visible="true" />    
        </asp:panel>
        
        
        
    
        
        

