<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Login to Irama Gemilang Lestari</title>   
   <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
  
  <script language="javascript" type="text/javascript">
  if (top.location != self.location) {
    top.location.replace(self.location);
  }
  </script>  

<script type="text/javascript">
  document.addEventListener("DOMContentLoaded", function () {
      var passwordInput = document.getElementById('<%= dbPassword.ClientID %>');
      var toggleIcon = document.getElementById('togglePassword');

      toggleIcon.addEventListener('click', function () {
          if (passwordInput.type === "password") {
              passwordInput.type = "text";
              toggleIcon.classList.remove("fa-eye");
              toggleIcon.classList.add("fa-eye-slash");
          } else {
              passwordInput.type = "password";
              toggleIcon.classList.remove("fa-eye-slash");
              toggleIcon.classList.add("fa-eye");
          }
      });
  });
</script>

    <style>
        /* HTML elements  *//*h1, h2, h3, h4, h5, h6*/
        /*{
            font-weight: normal;
            margin: 0;
            line-height: 1.1em; 
            
            color: #000;
            font-family: Roboto;
            font-weight: bold;
        }*/
        h1 {
              text-align: center;
              font-size: 30px;
              font-weight: 600;
              text-transform: uppercase;
              display:inline-block;
              margin: 40px 8px 10px 8px; 
              padding-left: 65px;
              color: #4f4f4f;
            }

        h1.active {
              color: #4f4f4f;
              border-bottom: 2px solid #5fbae9;
              bor
            }

            h1.active2 {
              color: #4f4f4f;
              border-bottom: 2px solid #5fbae9;
              border-right: 100px;
            }

         .TextBox
            {
                background-color: #f6f6f6;
                padding-left: 5px;
                padding-right: 5px;
                font-family: Roboto;
                font-size:13px;
                height:     35px;
                width: 300px;
                border: 0px solid #ccc;
                border-radius: 10px;
                color: #333333;
                outline: none;  
                border: 2px solid #f6f6f6;
                text-align: center;
              text-decoration: none;
              display: inline-block;
              -webkit-transition: all 0.5s ease-in-out;
              -moz-transition: all 0.5s ease-in-out;
              -ms-transition: all 0.5s ease-in-out;
              -o-transition: all 0.5s ease-in-out;
              transition: all 0.5s ease-in-out;
              -webkit-border-radius: 5px 5px 5px 5px;
              border-radius: 5px 5px 5px 5px;            
            }

            .TextBox:focus {
                border-style: none;
              background-color: #fff;
              border: 2px solid #f6f6f6;
              border-bottom: 2px solid #5fbae9;
            }

            .TextBox:placeholder {
              color: #cccccc;
            }

       
       
        /* //  HTML elements *//* base */body, table, input, textarea, select, li, button
        {
            /*font: 1em "Lucida Sans Unicode" , "Lucida Grande" , sans-serif;*/
            /*line-height: 1.5em;
            */
            color: #444;
            font-family: Roboto;
        }        
        body
        {
            /*font-size: 13px;*/
            /*background: #FFF; 
                       */
                       background-image: url(Image/backgroundlog.png);
            /*text-align: center;*/
        }

        /* // base *//* formsauth form */
        #formsauth
        {
            margin: 12em auto;
            background: #FFF;
            /*background-image: url(Image/login.png);*/
            /*border: 8px solid #eee;*/
            width: 800px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 20px;
            -moz-box-shadow: 0 0 20px #787878;
            -webkit-box-shadow: 0 0 10px #787878;
            box-shadow: 0 0 20px #787878;

            /* -webkit-box-shadow: 0 10px 30px 0 rgba(95,186,233,0.4);
            box-shadow: 0 10px 30px 0 rgba(95,186,233,0.4);*/
            text-align: left;
            position: relative;  
            height: 450px;          
        }
      
        /*#formsauth h1
        {*/
            /*background: #0092c8;*/
            /*color: #fff;
            text-shadow: #007dab 0 1px 0;
            font-size: 24px;
            padding: 18px 23px;
            margin: 0 0 .5em 0;*/
           
        /*}
        #formsauth p
        {
            margin: .5em 25px;
        }*/
        #formsauth div
        {
            /*margin: .5em ;*/
            /*background: #0092c8;*/
            margin-left: 35px;
            margin-bottom: 20px;

            /*-moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            */
            text-align: left;
            position: relative;
        }
        #formsauth label
        {
            float: left;
            line-height: 30px;
            padding-left: 10px;
             font-family: Roboto;
        }
        #formsauth .field
        {
            outline: none;  
            border: 2px solid #f6f6f6;
            font-family: Roboto;  
            background-color: #f6f6f6;  
            font-size: 12px;
            height:     35px;
            width: 315px;
            padding-left: 5px;
            padding-right: 10px;
            border: 0px solid #ccc;
            border-radius: 5px;
            resize: vertical;
            color: #333333;
            /*-moz-box-shadow: inset 0 0 5px #ccc;
            -webkit-box-shadow: inset 0 0 5px #ccc;
            box-shadow: inset 0 0 5px #ccc;*/
        }

.login{
     background-repeat: no-repeat;
       background-image: url(Image/icon-save-new.png);
       background-position:left; 
  padding-top: 10px;
  padding-bottom: 10px;
  padding-left: 40px;
  padding-right: 40px;
  border-radius: 10px;
  background-color: #3c8bc3;
  font-weight: bold;
  text-transform: uppercase;
  color: #FFFFFF;
  text-align: center;
  font-size: 13px;
  transition: all 1s;
  cursor: pointer;
  text-decoration: none;
}

.login span {
  cursor: pointer;
  display: inline-block;
  position: relative;
  transition: 1s;
}

.login span:after {
  content: '\00bb';
  position: absolute;
  opacity: 0;
  top: 0;
  right: -20px;
  transition: 0.5s;
}

.login:hover span {
  padding-right: 25px;
}

.login:hover span:after {
  opacity: 1;
  right: 0;
}

.reset {
    background-repeat: no-repeat;
       background-image: url(Image/icon-delete.png);
       background-position:left; 
  padding-top: 10px;
  padding-bottom: 10px;
  text-transform: uppercase;
  padding-left: 40px;
  padding-right: 40px;
  border-radius: 10px;
  background-color: #e36e6e;
  font-weight: bold;
  font-family: Roboto;
  border: none;
  color: #FFFFFF;
  text-align: center;
  font-size: 13px;
  width: 100px;
  transition: all 1s;
  cursor: pointer;
  margin: 0px;
  text-decoration: none;
}

.reset span {
  cursor: pointer;
  display: inline-block;
  position: relative;
  transition: 1s;
}

.reset span:after {
  content: '\00bb';
  position: absolute;
  opacity: 0;
  top: 0;
  right: -20px;
  transition: 0.5s;
}

.reset:hover span {
  padding-right: 25px;
}

.reset:hover span:after {
  opacity: 1;
  right: 0;
}

.label{
 font-family: Roboto; 
font-size: 16px;
color: #333333;       
font-weight: bold;    
padding-bottom: 16px;
}

.formInput{
    margin-top: 50px;
}


.submit{
    margin-top: 40px;
}

               
    </style>
</head>
<body>
    <form class="formContent" defaultbutton="bSubmit" id="formsauth" method="post" runat="server">
    <input type="hidden" id = "hdpinstance" runat="server" value="default" />    	
    <h1 class="active">
        <!-- <asp:Image id="imgLogin" runat="server" Height="55px" 
                ImageUrl="~/Image/login_i       con.gif" Width="55px" /> &nbsp  -->
        <!-- <span>Log in access...</span> -->
        SIGN IN
    </h1> 
   <div class="formInput">    
    <div class="form">  
        <table>
            <tr>
               
                <!-- <td class="label" width="100px" align="left">User ID</td>
                <td width="8px"></td> -->
                <td>        
                <asp:TextBox placeholder="User ID" CssClass="TextBox" runat="server" ID="dbUser"></asp:TextBox>
                </td>
            </tr>
        </table>
        
    </div>
    <div>
        <table>
        <tr>
            
                <!-- <td class="label" width="100px" align="left">Password</td>
        <td width="8px"></td> -->
        

        <td>
          <asp:TextBox placeholder="Password" CssClass="TextBox" runat="server" ID="dbPassword" TextMode="Password"></asp:TextBox>
          <i class="fa fa-eye" id="togglePassword" style="cursor: pointer; margin-left: -35px; color: #888;"></i>
      </td>
        </tr>
        </table>  
    </div>    
    <div>
        <table>
        <tr>
            
        <!-- <td class="label" width="100px" align="Left">Instance</td>
        <td width="8px"></td> -->
        <td>
        <asp:DropDownList  ID="ddlServer" runat="server" class="field required" >
                            <asp:ListItem Selected="True">IGL</asp:ListItem>                            
                        </asp:DropDownList>
                        
        <%--<asp:TextBox placeholder="Company" CssClass="TextBox" runat="server" ID="ddlServer"  
                ></asp:TextBox>--%>
        </td>
        </tr>
        </table>
        
    </div>
    <div class="submit">
        <table>
        <tr>
        
        </td>
            <td >
            <asp:LinkButton CssClass="login" ID="bSubmit" runat="server"  > <span>Login</span> </asp:LinkButton> 
            </td>
                <td width="70px">
            <td >
                <asp:LinkButton CssClass="reset" ID="bReset" runat="server"  > <span>Reset</span> </asp:LinkButton>                 
             </td>
        </tr>
        </table>
           
    </div>
     <p class="back">
        <asp:Label runat="server" ID="lStatus"></asp:Label>  
    </p>
   </div> 

    </form>

</body>
</html>
