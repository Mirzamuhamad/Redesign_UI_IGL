Imports System.Data

Partial Class AccountingPeriod
    Inherits System.Web.UI.Page
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                If Not Request.QueryString("KeyId") = Nothing Then
                    If Not Session(Request.QueryString("KeyId")) Is Nothing Then
                        InitProperty()
                    End If
                End If
                'If Session("GLYear") Is Nothing Then
                '    Session("GLYear") = SQLExecuteScalar("SELECT Year FROM GLYear WHERE CurrentYear='Y'")
                '    Session("GLPeriod") = SQLExecuteScalar("SELECT Period FROM GLPeriod WHERE DefaultPeriod='Y'")
                'End If

                FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection").ToString)
                FillCombo(ddlMonth, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection").ToString)
                ddlYear.SelectedValue = ViewState("GLYear")
                ddlMonth.SelectedValue = ViewState("GLPeriod")
            End If
        Catch ex As Exception
            lbStatus.Text = "Page Load Error :" + ex.ToString
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

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Try
            If Not Session(Request.QueryString("KeyId")) Is Nothing Then
                'Dim UserProperty As DataRow
                'UserProperty = Session(Request.QueryString("KeyId"))
                'UserProperty.BeginEdit()
                'UserProperty("Year") = ddlYear.SelectedValue
                'UserProperty("Period") = ddlMonth.SelectedValue
                'UserProperty("PeriodName") = ddlMonth.SelectedItem.Text
                'UserProperty.EndEdit()
                'Session(Request.QueryString("KeyId")) = UserProperty
                Session(Request.QueryString("KeyId"))("Year") = ddlYear.SelectedValue
                Session(Request.QueryString("KeyId"))("Period") = ddlMonth.SelectedValue
                Session(Request.QueryString("KeyId"))("PeriodName") = ddlMonth.SelectedItem.Text
            End If
            'Session("GLYear") = ddlYear.SelectedValue
            'Session("GLPeriod") = ddlMonth.SelectedValue
            Response.Write("<script language='javascript'> {window.opener.updateAcc(" + QuotedStr(ddlYear.SelectedValue) + ", " + QuotedStr(ddlMonth.SelectedItem.Text) + "); window.close();}</script>")
        Catch ex As Exception
            lbStatus.Text = "btn OK Error : " + ex.ToString
        End Try
    End Sub
End Class
