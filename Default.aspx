<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

  <!DOCTYPE html>
  <html xmlns="http://www.w3.org/1999/xhtml">

  <head runat="server">
    <title>Login to Irama Gemilang Lestari</title>

    <!-- Bootstrap 5 -->
            <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
            <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
            <link href="Styles/StyleNew.css" rel="stylesheet" type="text/css" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style>
      body {
        height: 100vh;
        /* font-family: 'Poppins', sans-serif; */
        background-color: #f9f9f9;
        display: flex;
        justify-content: center;
        align-items: center;
      }

      .login-wrapper {
        display: flex;
        max-width: 900px;
        width: 100%;
        background: #fff;
        border-radius: 1rem;
        overflow: hidden;
        box-shadow: 0px 8px 25px rgba(0, 0, 0, 0.15);
        padding: 10px;
      }

      /* Bagian kiri */
      .login-left {
        /* background-color: #1e51dc; */
        background: linear-gradient(135deg, #1e51dc, #5a8bff); /* biru tua ke biru muda */
        color: #fff;
        flex: 1;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 4rem;
        text-align: center;
        border-radius: 10px;
        box-shadow: 0 4px 18px rgba(65, 65, 65, 0.2);
    transition: all 0.3s ease-in-out;
      }

      .login-left h3 {
        font-weight: 700;
        margin-bottom: 1rem;
      }

      .login-left p {
        font-size: 0.9rem;
        opacity: 0.9;
      }

      /* Bagian kanan (form) */
      .login-right {
        flex: 1;
        padding: 3rem;
        width: 500px;
      }

      .login-right h3 {
        font-weight: 700;
        margin-bottom: 0.5rem;
        color: #777;
      }

      .login-right p {
        color: #6c757d;
        font-size: 0.9rem;
        margin-bottom: 1.5rem;
      }

      .form-control:focus {
        border-color: #1e51dc;
        box-shadow: 0 0 0 0.2rem rgba(30, 81, 220, 0.25);
      }

      .btn-login {
        background-color: #1e51dc;
        color: #fff;
        font-weight: 500;
        font-size: 14px;
      }

      .btn-login:hover {
        background-color: #163cb5;
        color: #e2e2e2;
      }

      .toggle-eye {
        cursor: pointer;
        position: absolute;
        right: 15px;
        top: 50%;
        transform: translateY(-50%);
        color: #888;
      }

      /* Responsif HP */
      @media (max-width: 768px) {

        .login-left {
        height: 80px; /* 🔹 kurangi tinggi di HP */
        padding: 1rem;
        /* border-radius: 10px 10px 0 0; */
    }
        .login-wrapper {
          flex-direction: column;
          max-width: 100%;
        }
        

        .login-left img{
          width: 150px;
        }      

        .login-right {
          padding: 1.5rem 0rem;
          width: 300px;
        }

        .btn-login {
          font-size: 12px;
          padding: 0.75rem;
        }
      }

      .spinner-border {
    width: 1rem;
    height: 1rem;
}
.btn[disabled] {
    opacity: 0.8;
    cursor: not-allowed;
    pointer-events: none;
}

    </style>

    <style>
  .position-relative .toggle-eye {
      position: absolute;
      top: 50%;
      right: 10px;
      transform: translateY(-50%);
      color: #888;
      cursor: pointer;
  }
  .position-relative .toggle-eye:hover {
      color: #0d6efd;
  }
</style>

<!-- Script -->
<script>
  document.addEventListener("DOMContentLoaded", function () {
      const togglePassword = document.getElementById("togglePassword");
      const passwordField = document.getElementById("<%= dbPassword.ClientID %>");
      const loginButton = document.getElementById("<%= bSubmit.ClientID %>");

      // 👁️ Show/Hide Password
      togglePassword.addEventListener("click", function () {
          const type = passwordField.getAttribute("type") === "password" ? "text" : "password";
          passwordField.setAttribute("type", type);
          this.classList.toggle("fa-eye");
          this.classList.toggle("fa-eye-slash");
      });

      // ⏎ Tekan Enter = Klik Login
      passwordField.addEventListener("keypress", function (event) {
          if (event.key === "Enter") {
              event.preventDefault();
              loginButton.click();
          }
      });
  });
</script>

<script>
  function showLoading(btn) {
      // Dapatkan elemen-elemen tombol
      var spinner = btn.querySelector('.spinner-border');
      var text = btn.querySelector('.btn-text');
      var icon = btn.querySelector('.icon-login');
  
      // Tampilkan spinner dan ubah teks
      spinner.classList.remove('d-none');
      icon.classList.add('d-none');
      text.textContent = 'Loading...';
      btn.disabled = true;
  
      // Jalankan postback ASP.NET dengan nama tombol yang benar
      setTimeout(function () {
          // Ambil UniqueID tombol (ASP.NET butuh ini agar handler bSubmit_Click jalan)
          __doPostBack('<%= bSubmit.UniqueID %>', '');
      }, 100);
  
      return false; // cegah postback default dulu
  }
  </script>
  
  
  </head>

  <body>
    <form id="formsauth" runat="server">
      <div class="login-wrapper">

        <!-- Kiri -->
        <div class="login-left">
          <img src="Image/Illustration-PNG-compressed.png" width="200" class="mb-0" />
          <!-- <h3>Be Verifiedsnced designers on this platform.</p> -->
        </div>

        <!-- Kanan -->
        <div class="login-right">
          <h3>Sign-in</h3>
          <br>

          <div class="mb-3">
            <asp:TextBox ID="dbUser" runat="server" CssClass="form-control form-control-sm" placeholder="User ID">
            </asp:TextBox>
          </div>

          <div class="mb-3 position-relative">
            <asp:TextBox ID="dbPassword" runat="server" CssClass="form-control form-control-sm" placeholder="Password"
              TextMode="Password"></asp:TextBox>
            <i class="fa fa-eye toggle-eye" id="togglePassword"></i>
          </div>

          <asp:LinkButton 
            ID="bSubmit" 
            runat="server"
            CssClass="btn btn-login btn-sm w-100 me-1 d-flex align-items-center justify-content-center position-relative"
            OnClientClick="return showLoading(this);">
            <span class="spinner-border spinner-border-sm me-2 d-none" role="status" aria-hidden="true"></span>
            <i class="fas fa-sign-in-alt me-2 icon-login"></i>
            <span class="btn-text">Login</span>
        </asp:LinkButton>


          <asp:LinkButton CssClass="btn btn-reset w-50 ms-2 d-flex align-items-center justify-content-center"
            Visible="false" ID="bReset" runat="server">
            <i class="fas fa-undo me-2"></i> Reset
          </asp:LinkButton>

          <div class="mb-3">
            <asp:DropDownList ID="ddlServer" runat="server" Visible="false" CssClass="form-select">
              <asp:ListItem Selected="True">IGL</asp:ListItem>
            </asp:DropDownList>
          </div>

          <div class="text-center mt-3">
            <asp:Label runat="server" ID="lStatus" CssClass="text-danger small"></asp:Label>
          </div>
        </div>
      </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
  </body>

  </html>