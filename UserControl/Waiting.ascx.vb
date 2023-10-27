
Partial Class UserControl_WebUserControl
    Inherits System.Web.UI.UserControl

    Public Sub Hide()
        mpextwait.Hide()
    End Sub

    Public Sub Show(ByVal Msg As String)
        lblText.Text = Msg + "..."
        mpextwait.Show()
    End Sub
End Class
