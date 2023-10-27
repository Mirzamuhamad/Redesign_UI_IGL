
Partial Class UserControl_PageControl
    Inherits System.Web.UI.UserControl
    'Public Event TextChangeCtrl As EventHandler
    Public Event TextBoxControl_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event TextBoxControl_BtnClick(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            tbpageNo.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            'lStatus.Text = "page load error : " + ex.ToString
        End Try
    End Sub

    Public Sub SetMaxPage(ByVal MaxPage As Integer)
        Try
            lbpageMax.Text = MaxPage.ToString
            tbpageNo.Text = "1"
        Catch ex As Exception
            'lStatus.Text = "Set Rpt Error : " + ex.ToString
        End Try
    End Sub

    Public Function getpageNo() As String
        Return tbpageNo.Text
    End Function

    Protected Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFirst.Click
        tbpageNo.Text = "1"
        RaiseEvent TextBoxControl_BtnClick(sender, e)
    End Sub

    Protected Sub btnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLast.Click
        If CInt(tbpageNo.Text) < CInt(lbpageMax.Text) Then
            tbpageNo.Text = lbpageMax.Text
            RaiseEvent TextBoxControl_BtnClick(sender, e)
        End If
    End Sub

    Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrev.Click
        If CInt(tbpageNo.Text) > 1 Then
            tbpageNo.Text = (CInt(tbpageNo.Text) - 1).ToString
            RaiseEvent TextBoxControl_BtnClick(sender, e)
        End If
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNext.Click
        Dim Max As Integer
        Max = CInt(lbpageMax.Text)
        If CInt(tbpageNo.Text) < Max Then
            tbpageNo.Text = (CInt(tbpageNo.Text) + 1).ToString
            RaiseEvent TextBoxControl_BtnClick(sender, e)
        End If
    End Sub

    Protected Sub tbpageNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbpageNo.TextChanged
        'RaiseEvent TextChangeCtrl(Me, New EventArgs)
        If CInt(tbpageNo.Text) > CInt(lbpageMax.Text) Then
            tbpageNo.Text = lbpageMax.Text
        End If
        RaiseEvent TextBoxControl_TextChanged(sender, e)
    End Sub
End Class
