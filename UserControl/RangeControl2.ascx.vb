
Partial Class UserControl_RangeControl2
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            tb1.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb2.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tb3.Attributes.Add("OnKeyDown", "return PressNumeric();")
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
    Public Sub ModifyRange1(ByVal tb2 As Integer)
        'Dim Currrange, C2, CS2 As Integer
        Try

            tbStart1.Text = (tb2 - 1).ToString

            'ModifyRange3(CInt(tb2.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 2 Error : " + ex.ToString)
        End Try
    End Sub
    Public Sub ModifyRange2(ByVal tb3 As Integer)
        'Dim Currrange, C3, CS3 As Integer
        Try
            'C3 = CInt(tb3.Text)
            'CS3 = CInt(tbStart3.Text)

            'Currrange = C3 - CS3
            tbStart2.Text = (tb3 - 1).ToString
            'tb3.Text = (tbv2 + 1 + Currrange).ToString

            'ModifyRange4(CInt(tb3.Text))
        Catch ex As Exception
            Throw New Exception("Modify Range 3 Error : " + ex.ToString)
        End Try
    End Sub
    Public Sub ModifyRange3(ByVal tb4 As Integer)
        Try
            tbStart3.Text = (tb4 - 1).ToString
            'tb4.Text = (tbv3 + 1).ToString
        Catch ex As Exception
            Throw New Exception("Modify Range 4 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tb1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb1.TextChanged
        Try
            If CInt(tb1.Text) >= CInt(tb2.Text) Then
                tbStart1.Text = tb1.Text
                tb2.Text = CStr(CInt(tb1.Text) + 1)
            End If
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
            If (CInt(tb2.Text) <= CInt(tb1.Text)) Then 'Or (CInt(tb2.Text) <= CInt(tbStart1.Text)) Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb2.Text) >= CInt(tb3.Text) Then
                tbStart1.Text = CStr(CInt(tb2.Text) - 1)
                tbStart2.Text = tb2.Text
                tb3.Text = CStr(CInt(tb2.Text) + 1)
            Else
                ModifyRange1(CInt(tb2.Text))
            End If

        Catch ex As Exception
            lStatus.Text = "tb 2Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub tb3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb3.TextChanged
        Try
            If CekTB() = False Then
                RevertValue()
                Exit Sub
            End If
            If (CInt(tb3.Text) <= CInt(tb2.Text)) Then 'Or (CInt(tb3.Text) <= CInt(tbStart2.Text)) Then
                RevertValue()
                Exit Sub
            End If
            If CInt(tb3.Text) >= CInt(tb4.Text) Then
                tbStart2.Text = CStr(CInt(tb3.Text) - 1)
                tbStart3.Text = tb3.Text
                tb4.Text = CStr(CInt(tb3.Text) + 1)
            Else
                ModifyRange2(CInt(tb3.Text))
            End If
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
            If (CInt(tb4.Text) <= CInt(tb3.Text)) Then 'Or (CInt(tb4.Text) <= CInt(tbStart3.Text)) Then
                RevertValue()
                Exit Sub
            End If
            ModifyRange3(CInt(tb4.Text))
        Catch ex As Exception
            lStatus.Text = "tb 4 Error : " + ex.ToString
        End Try
    End Sub

    Private Function CekTB() As Boolean
        Try
            If (Not IsNumeric(tb1.Text.Trim)) Or (Not IsNumeric(tb2.Text.Trim)) Or (Not IsNumeric(tb3.Text.Trim)) Or (Not IsNumeric(tb4.Text.Trim)) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw New Exception("Cek TB Error : " + ex.ToString)
        End Try
    End Function

    Private Sub RevertValue()
        Dim A, B, C As Integer ', D
        Try
            A = CInt(tbStart1.Text) + 1
            B = CInt(tbStart2.Text) + 1
            C = CInt(tbStart3.Text) + 1

            tb2.Text = A.ToString
            tb3.Text = B.ToString
            tb4.Text = C.ToString
        Catch ex As Exception
            lStatus.Text = "Revert Value Error : " + ex.ToString
        End Try
    End Sub

    
End Class
