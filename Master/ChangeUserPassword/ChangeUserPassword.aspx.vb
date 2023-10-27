Imports System.Data

Partial Class Master_ChangeUserPassword_ChangeUserPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                lbUser.Text = ViewState("UserId")
            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "page Load Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub InitProperty()
        ViewState("DBConnection") = Session(Request.QueryString("KeyId"))("DBConnection")
        ViewState("UserId") = Session(Request.QueryString("KeyId"))("UserId")
        ViewState("UserName") = Session(Request.QueryString("KeyId"))("UserName")
        ViewState("FgAdmin") = Session(Request.QueryString("KeyId"))("FgAdmin")
        ViewState("Currency") = Session(Request.QueryString("KeyId"))("Currency")
        ViewState("GLYear") = Session(Request.QueryString("KeyId"))("Year")
        ViewState("GLPeriod") = Session(Request.QueryString("KeyId"))("Period")
        ViewState("GLPeriodName") = Session(Request.QueryString("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Request.QueryString("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Request.QueryString("KeyId"))("Address1")
        ViewState("Address2") = Session(Request.QueryString("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Request.QueryString("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Request.QueryString("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Request.QueryString("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Request.QueryString("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Request.QueryString("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Request.QueryString("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            tbNewPassword.Text = ""
            tbOldPassword.Text = ""
            tbRetypePassword.Text = ""
        Catch ex As Exception
            lbStatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As String
        Try
            If tbNewPassword.Text.Length = 0 Or tbNewPassword.Text.Length = 0 Or tbRetypePassword.Text.Length = 0 Then
                lbStatus.Text = MessageDlg("All field must be filled.")
                tbNewPassword.Focus()
                Exit Sub
            End If

            If tbNewPassword.Text.Trim <> tbRetypePassword.Text.Trim Then
                lbStatus.Text = MessageDlg("New password is not match")
                tbNewPassword.Focus()
                Exit Sub
            End If

            result = SQLExecuteScalar("Declare @A VARCHAR(255) EXEC S_SAUsersChangePassword " + QuotedStr(ViewState("UserId")) + ", " + _
            QuotedStr(tbOldPassword.Text) + ", " + QuotedStr(tbNewPassword.Text) + ", @A OUT " + _
            "SELECT @A", ViewState("DBConnection").ToString)

            If result.Length > 2 Then
                lbStatus.Text = MessageDlg(result)
            Else
                lbStatus.Text = MessageDlg("Update Password Success!")
            End If

        Catch ex As Exception
            lbStatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub
End Class
