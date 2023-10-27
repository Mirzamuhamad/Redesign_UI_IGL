
Partial Class Rpt_RptExpSummary_RptExpSummary
    Inherits System.Web.UI.Page
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session("PrintType") = "Print"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
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
        ViewState("ServerDate") = Session(Request.QueryString("KeyId"))("ServerDate")
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlYear, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriod, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlYearEnd, "EXEC S_GetYeaR", False, "Year", "Year", ViewState("DBConnection"))
                FillCombo(ddlPeriodEnd, "EXEC S_GetPeriod", False, "Period", "Description", ViewState("DBConnection"))
                FillCombo(ddlExpense, "SELECT * FROM VMsAccType WHERE FgType = 'PL'", False, "AccTypeCode", "AccTypeName", ViewState("DBConnection").ToString)

                ddlYear.SelectedValue = ViewState("GLYear").ToString
                ddlYearEnd.SelectedValue = ViewState("GLYear").ToString
                ddlPeriod.SelectedValue = ViewState("GLPeriod").ToString
                ddlPeriodEnd.SelectedValue = ViewState("GLPeriod").ToString
            End If
            lbStatus.Text = ""
        Catch ex As Exception
            lbStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    Private Sub print()
        Dim CekPeriod, ReportType As String
        Try
            CekPeriod = CekRangePeriodSelected(ddlYear.SelectedValue, ddlPeriod.SelectedValue, ddlYearEnd.SelectedValue, ddlPeriodEnd.SelectedValue)
            If cekPeriod <> "" Then
                lbStatus.Text = MessageDlg(cekPeriod)
                Exit Sub
            End If

            If IsNumeric(tbPeriod.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Range must be in numeric.")
                tbPeriod.Focus()
                Exit Sub
            End If
            If Request.QueryString("ContainerId").ToString = "RptExpSummaryExId" Then
                ReportType = "1"
            Else
                ReportType = "0"
            End If
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_GLRptExpenseSummary " + ddlYear.SelectedValue + _
            "," + ddlPeriod.SelectedValue + ", " + ddlPeriodX.SelectedValue + ", " + tbPeriod.Text + "," + ddlYearEnd.SelectedValue + _
            "," + ddlPeriodEnd.SelectedValue + "," + QuotedStr(ddlExpense.SelectedValue) + "," + rbType.SelectedIndex.ToString + ", " + ReportType

            'lbStatus.Text = Session("SelectCommand")
            'Exit Sub

            Session("ReportFile") = Server.MapPath("~\Rpt\RptExpSummary.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            Throw New Exception("print error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Session("PrintType") = "Preview"
            print()
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub tbPeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPeriod.TextChanged, ddlPeriodX.SelectedIndexChanged, ddlPeriod.SelectedIndexChanged, ddlYear.SelectedIndexChanged
        Dim range, tahun, bulan, tahun2, bulan2 As Integer
        Try
            If (CInt(tbPeriod.Text) = 0) Then
                tbPeriod.Text = "1"
            End If
            range = (CInt(ddlPeriodX.SelectedValue) * CInt(tbPeriod.Text)) - 1
            tahun = CInt(ddlYear.SelectedValue)
            bulan = CInt(ddlPeriod.SelectedValue)
            bulan2 = bulan + range
            If bulan2 > 24 Then
                bulan2 = bulan2 - 24
                tahun2 = tahun + 2
            ElseIf bulan2 > 12 Then
                bulan2 = bulan2 - 12
                tahun2 = tahun + 1
            Else
                tahun2 = tahun
            End If
            ddlYearEnd.SelectedValue = tahun2.ToString
            ddlPeriodEnd.SelectedValue = bulan2.ToString
        Catch ex As Exception
            lbStatus.Text = "tbPeriod_TextChanged Error : " + ex.ToString
        End Try
    End Sub
End Class