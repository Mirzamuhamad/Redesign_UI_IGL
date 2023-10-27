
Partial Class UserControl_MsgBox
    Inherits System.Web.UI.UserControl
    Dim HasilClik As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnYes.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btnYes.UniqueID, "")
        btnNo.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btnNo.UniqueID, "")
    End Sub

    Private Sub Hide()
        lblMessage.Text = ""
        lblCaption.Text = ""
        mpext.Hide()
    End Sub

    Public Function GetResult() As Boolean
        If HasilClik = "Y" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub MsgBox(ByVal MessageType As String, ByVal Caption As String)
        lblCaption.Text = MessageType
        lblMessage.Text = Caption
        If (MessageType = "Confirmation") Then
            btnYes.Text = "Yes"
            btnNo.Text = "No"
            btnNo.Visible = True
        Else
            btnYes.Text = "OK"
            btnNo.Visible = False
        End If
        mpext.Show()
    End Sub

    'Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
    '    HasilClik = "N"
    '    mpext.Hide()
    'End Sub

    'Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
    '    HasilClik = "Y"
    '    mpext.Hide()
    'End Sub

    Public Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        OnYesButtonPressed(e)
    End Sub
    

    Public Delegate Sub YesButtonPressedHandler(ByVal sender As Object, ByVal args As System.EventArgs)
    Public Event YesButtonPressed As YesButtonPressedHandler

    Protected Overridable Sub OnYesButtonPressed(ByVal e As System.EventArgs)
        If Not (e Is Nothing) Then
            RaiseEvent YesButtonPressed(btnYes, e)
        End If
    End Sub

    Public Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        OnNoButtonPressed(e)
    End Sub


    Public Delegate Sub NoButtonPressedHandler(ByVal sender As Object, ByVal args As System.EventArgs)
    Public Event NoButtonPressed As NoButtonPressedHandler

    Protected Overridable Sub OnNoButtonPressed(ByVal e As System.EventArgs)
        If Not (e Is Nothing) Then
            RaiseEvent NoButtonPressed(btnYes, e)
        End If
    End Sub
End Class
