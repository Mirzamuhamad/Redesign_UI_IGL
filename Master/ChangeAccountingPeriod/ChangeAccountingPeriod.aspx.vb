
Partial Class Master_ChangeAccountingPeriod_ChangeAccountingPeriod
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                'If ViewState("GLYear") Is Nothing Then
                ViewState("GLYear") = SQLExecuteScalar("SELECT Year FROM GLYear WHERE CurrentYear='Y'", ViewState("DBConnection").ToString)
                ViewState("GLPeriod") = SQLExecuteScalar("SELECT Period FROM GLPeriod WHERE DefaultPeriod='Y'", ViewState("DBConnection").ToString)
                'End If
                FillCombo(ddlYear, "EXEC S_GetYear", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlMonth, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
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
        Dim SQLString As String
        Try
            If ddlYear.SelectedValue <> ViewState("GLYear") Then
                SQLString = "Update GLYear Set CurrentYear = 'N' WHERE CurrentYear = 'Y'"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                SQLString = "UPDATE GLYear Set CurrentYear = 'Y' WHERE Year=" + ddlYear.SelectedValue
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            End If

            If ddlMonth.SelectedValue <> ViewState("GLPeriod") Then
                SQLString = "Update GLPeriod Set DefaultPeriod = 'N' WHERE DefaultPeriod = 'Y'"
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
                SQLString = "UPDATE GLPeriod Set DefaultPeriod = 'Y' WHERE Period=" + ddlMonth.SelectedValue
                SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
            End If
            lbStatus.Text = MessageDlg("Update data success!.")
        Catch ex As Exception
            lbStatus.Text = "btn OK Error : " + ex.ToString
        End Try
    End Sub

End Class
