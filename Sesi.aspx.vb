
Partial Class Sesi
    Inherits System.Web.UI.Page

    Protected Sub btnSesi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSesi.Click
        Session.Clear()
        Session.Abandon()
        Response.Write("<script>window.top.location.href='Default.aspx';</script>")
    End Sub

End Class
