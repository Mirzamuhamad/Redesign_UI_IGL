
Partial Class UserControl_RangeControl
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            tb1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb3.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb4.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb5.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb6.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
    Public Function getRange5() As String
        Return tb5.Text
    End Function
    Public Function getRange6() As String
        Return tb6.Text
    End Function
    Public Function getRange7() As String
        Return tb7.Text
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

    Public Sub ModifyRange4(ByVal tbv3 As Integer)
        Dim Currrange, C4, CS4 As Integer
        Try
            'tb4.Text = (tbv3 + 1).ToString
            C4 = CInt(tb4.Text)
            CS4 = CInt(tbStart4.Text)

            Currrange = C4 - CS4
            tbStart4.Text = (tbv3 + 1).ToString
            tb4.Text = (tbv3 + 1 + Currrange).ToString

            ModifyRange5(CInt(tb4.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 4 Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyRange5(ByVal tbv4 As Integer)
        Dim Currrange, C5, CS5 As Integer
        Try
            C5 = CInt(tb5.Text)
            CS5 = CInt(tbStart5.Text)

            Currrange = C5 - CS5
            tbStart5.Text = (tbv4 + 1).ToString
            tb5.Text = (tbv4 + 1 + Currrange).ToString

            ModifyRange6(CInt(tb5.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 5 Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyRange6(ByVal tbv5 As Integer)
        Dim Currrange, C6, CS6 As Integer
        Try
            C6 = CInt(tb6.Text)
            CS6 = CInt(tbStart6.Text)

            Currrange = C6 - CS6
            tbStart6.Text = (tbv5 + 1).ToString
            tb6.Text = (tbv5 + 1 + Currrange).ToString

            ModifyRange7(CInt(tb6.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 6 Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyRange7(ByVal tbv6 As Integer)
        Try
            tb7.Text = (tbv6 + 1).ToString
        Catch ex As Exception
            Throw New Exception("Modify Range 7 Error : " + ex.ToString)
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

    Protected Sub tb4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb4.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb4.Text) <= CInt(tbStart4.Text) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange5(CInt(tb4.Text))
        Catch ex As Exception
            lStatus.Text = "tb 4 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tb5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb5.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb5.Text) <= CInt(tbStart5.Text) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange6(CInt(tb5.Text))
        Catch ex As Exception
            lStatus.Text = "tb 5 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tb6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb6.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb6.Text) <= CInt(tbStart6.Text) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange7(CInt(tb6.Text))
        Catch ex As Exception
            lStatus.Text = "tb 6 Error : " + ex.ToString
        End Try
    End Sub

    Private Function CekTB() As Boolean
        Try
            If (Not IsNumeric(tb1.Text.Trim)) Or (Not IsNumeric(tb2.Text.Trim)) Or (Not IsNumeric(tb3.Text.Trim)) Or (Not IsNumeric(tb4.Text.Trim)) Or (Not IsNumeric(tb5.Text.Trim)) Or (Not IsNumeric(tb6.Text.Trim)) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek TB Error : " + ex.ToString)
        End Try
    End Function

    Private Sub RevertValue()
        Dim Start2, Start3, Start4, Start5, Start6, A, B, C, D, E, F, G As Integer
        Try
            Start2 = CInt(tbStart2.Text)
            Start3 = CInt(tbStart3.Text)

            Start4 = CInt(tbStart4.Text)
            Start5 = CInt(tbStart5.Text)
            Start6 = CInt(tbStart6.Text)

            'D = CInt(tb4.Text)
            G = CInt(tb7.Text)

            A = Start2 - 1
            B = Start3 - 1

            C = Start4 - 1
            D = Start5 - 1
            E = Start6 - 1

            'C = D - 1
            F = G - 1

            tb1.Text = A.ToString
            tb2.Text = B.ToString
            tb3.Text = C.ToString

            tb4.Text = D.ToString
            tb5.Text = E.ToString
            tb6.Text = F.ToString
        Catch ex As Exception
            lStatus.Text = "Revert Value Error : " + ex.ToString
        End Try
    End Sub
End Class
