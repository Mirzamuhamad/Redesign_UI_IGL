
Partial Class UserControl_RangeControl4
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            tb1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb4.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            lStatus.Text = "page load error : " + ex.ToString
        End Try
    End Sub

    Public Function getRange1() As String
        Return tb1.Text
    End Function

    Public Function getRange2() As String
        Return tb2.Text
    End Function

    Public Function getRange3() As String
        Return tb3.Text
    End Function

    Public Function getRange4() As String
        Return tb4.Text
    End Function

    Public Sub ModifyRange2(ByVal tb1 As Integer)
        Dim Currrange, C2, CS2 As Integer
        Try
            C2 = CInt(tb2.Text)
            CS2 = CInt(tbStart2.Text)

            Currrange = C2 - CS2
            tbStart2.Text = (tb1 + 1).ToString
            tb2.Text = (tb1 + 1 + Currrange).ToString

            ModifyRange3(CInt(tb2.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 2 Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyRange3(ByVal tbv2 As Integer)
        Dim Currrange, C3, CS3 As Integer
        Try
            C3 = CInt(tb3.Text)
            CS3 = CInt(tbStart3.Text)

            Currrange = C3 - CS3
            tbStart3.Text = (tbv2 + 1).ToString
            tb3.Text = (tbv2 + 1 + Currrange).ToString

            ModifyRange4(CInt(tb3.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 3 Error : " + ex.ToString)
        End Try
    End Sub


    Public Sub ModifyRange4(ByVal tbv4 As Integer)
        Try
            tb4.Text = (tbv4 + 1).ToString
        Catch ex As Exception
            Throw New Exception("Modify Range 5 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tb1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb1.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange2(CInt(tb1.Text))
        Catch ex As Exception
            lStatus.Text = "tb 1 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tb2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb2.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb2.Text) <= CInt(tbStart2.Text) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange3(CInt(tb2.Text))
        Catch ex As Exception
            lStatus.Text = "tb 2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tb3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb3.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb3.Text) <= CInt(tbStart3.Text) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange4(CInt(tb3.Text))
        Catch ex As Exception
            lStatus.Text = "tb 3 Error : " + ex.ToString
        End Try
    End Sub

    Private Function CekTB() As Boolean
        Try
            If (Not IsNumeric(tb1.Text.Trim)) Or (Not IsNumeric(tb2.Text.Trim)) Or (Not IsNumeric(tb3.Text.Trim)) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek TB Error : " + ex.ToString)
        End Try
    End Function

    Private Sub RevertValue()
        Dim Start2, Start3, A, B, C, D, E As Integer
        Try
            Start2 = CInt(tbStart2.Text)
            Start3 = CInt(tbStart3.Text)
            E = CInt(tb4.Text)
            A = Start2 - 1
            B = Start3 - 1
            D = E - 1

            tb1.Text = A.ToString
            tb2.Text = B.ToString
            tb3.Text = C.ToString
        Catch ex As Exception
            lStatus.Text = "Revert Value Error : " + ex.ToString
        End Try
    End Sub

End Class
