<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Sesi.aspx.vb" Inherits="Sesi"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sesi</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- Bootstrap JS (wajib jika pakai komponen interaktif seperti modal, dropdown, dll) -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
<link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
<link href="Styles/StyleNew.css" rel="stylesheet" type="text/css" />

</head>
<body>
    
    <form id="form1" runat="server">
   <div class="container d-flex align-items-center justify-content-center min-vh-100">
    <div class="card shadow-lg border-0 text-center p-4" style="max-width: 450px; border-radius: 1rem;">
        <div class="mb-3">
            <img src="image/session2.png" alt="Session Expired" class="img-fluid" style="max-width: 250px;">
        </div>
        <h5 class="text-danger fw-bold mb-1">Upss... your session has expired!</h5>
        <asp:LinkButton runat="server" ID="btnSesi" CssClass="btn btn-primary btn-sm w-100 fw-bold py-2 text-white">
    <i class="bi bi-box-arrow-in-right me-2"></i> Back to Log-In
</asp:LinkButton>

    </div>
</div>

    </form>
</div>
</body>
</html>
